using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BackendSport.Application.Services;
using BackendSport.Domain.Entities.AuthEntities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BackendSport.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(User user)
        {
            var keyString = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(keyString))
                throw new InvalidOperationException("La clave JWT no está configurada en appsettings.json (Jwt:Key)");
            var key = Encoding.ASCII.GetBytes(keyString);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            if (user.RolIds != null)
            {
                foreach (var rol in user.RolIds)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken(User user)
        {
            var guid = Guid.NewGuid().ToString();
            var tokenSecret = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return $"{guid}.{tokenSecret}";
        }

        public string? GetUserIdFromAccessToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(accessToken);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");
            return userIdClaim?.Value;
        }
    }
} 