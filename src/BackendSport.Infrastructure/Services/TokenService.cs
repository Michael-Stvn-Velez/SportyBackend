using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BackendSport.Application.Services;
using BackendSport.Domain.Entities.AuthEntities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace BackendSport.Infrastructure.Services
{
    /// <summary>
    /// Servicio para la gestión de tokens JWT con implementación de mejores prácticas de seguridad.
    /// Proporciona funcionalidades para generar, validar y gestionar access tokens y refresh tokens.
    /// </summary>
    /// <remarks>
    /// Este servicio implementa:
    /// - Generación segura de tokens con claims personalizados
    /// - Validación robusta de tokens
    /// - Refresh token rotation
    /// - Claims para roles y deportes del usuario
    /// - JWT ID para blacklisting
    /// </remarks>
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IOptions<JwtSettings> _jwtOptions;

        /// <summary>
        /// Inicializa una nueva instancia del servicio de tokens.
        /// </summary>
        /// <param name="jwtOptions">Configuración de JWT inyectada por DI</param>
        public TokenService(IOptions<JwtSettings> jwtOptions)
        {
            _jwtOptions = jwtOptions;
            _jwtSettings = jwtOptions.Value;
        }

        /// <summary>
        /// Genera un access token JWT para el usuario especificado.
        /// </summary>
        /// <param name="user">Usuario para el cual generar el token</param>
        /// <returns>Access token JWT como string</returns>
        /// <exception cref="InvalidOperationException">Se lanza cuando la configuración JWT es inválida</exception>
        /// <remarks>
        /// El token incluye los siguientes claims:
        /// - User ID (nameid)
        /// - Email
        /// - Name
        /// - JWT ID (jti) para blacklisting
        /// - Issued at (iat)
        /// - Subject (sub)
        /// - Email verified
        /// - Roles (si existen)
        /// - Sports (si existen)
        /// </remarks>
        public string GenerateAccessToken(User user)
        {
            if (string.IsNullOrEmpty(_jwtSettings.SecretKey))
                throw new InvalidOperationException("La clave JWT no está configurada");

            var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);
            if (key.Length < 32)
                throw new InvalidOperationException("La clave JWT debe tener al menos 32 caracteres");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name ?? string.Empty),
                new Claim("jti", Guid.NewGuid().ToString()), // JWT ID para blacklisting
                new Claim("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim("sub", user.Id),
                new Claim("email_verified", "true", ClaimValueTypes.Boolean)
            };

            // Agregar roles como claims
            if (user.RolIds != null && user.RolIds.Any())
            {
                foreach (var rol in user.RolIds)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol));
                }
            }

            // Agregar deportes como claims personalizados
            if (user.Sports != null && user.Sports.Any())
            {
                var sportsClaim = string.Join(",", user.Sports.Select(s => s.SportId));
                claims.Add(new Claim("sports", sportsClaim));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha512Signature),
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Genera un refresh token seguro para el usuario especificado.
        /// </summary>
        /// <param name="user">Usuario para el cual generar el refresh token</param>
        /// <returns>Refresh token como string en formato "tokenId.tokenSecret"</returns>
        /// <remarks>
        /// El refresh token se genera usando RandomNumberGenerator para mayor seguridad.
        /// El formato es "tokenId.tokenSecret" donde ambos componentes son criptográficamente seguros.
        /// </remarks>
        public string GenerateRefreshToken(User user)
        {
            // Generar un token más seguro usando RandomNumberGenerator
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            var tokenId = Guid.NewGuid().ToString();
            var tokenSecret = Convert.ToBase64String(randomBytes);
            
            return $"{tokenId}.{tokenSecret}";
        }

        /// <summary>
        /// Extrae el ID del usuario desde un access token JWT.
        /// </summary>
        /// <param name="accessToken">Access token JWT</param>
        /// <returns>ID del usuario o null si no se puede extraer</returns>
        /// <remarks>
        /// Este método no valida el token, solo extrae el claim del user ID.
        /// Para validación completa, use ValidateAccessToken.
        /// </remarks>
        public string? GetUserIdFromAccessToken(string accessToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                
                if (!handler.CanReadToken(accessToken))
                    return null;

                var jwtToken = handler.ReadJwtToken(accessToken);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                
                return userIdClaim?.Value;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Valida completamente un access token JWT.
        /// </summary>
        /// <param name="accessToken">Access token JWT a validar</param>
        /// <returns>ClaimsPrincipal si el token es válido, null en caso contrario</returns>
        /// <remarks>
        /// Esta validación incluye:
        /// - Verificación de la firma
        /// - Validación del emisor y audiencia
        /// - Verificación de la expiración
        /// - Validación del tiempo de vida
        /// </remarks>
        public ClaimsPrincipal? ValidateAccessToken(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.SecretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = _jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = _jwtSettings.ValidateIssuer,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = _jwtSettings.ValidateAudience,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = _jwtSettings.ValidateLifetime,
                    ClockSkew = TimeSpan.FromMinutes(_jwtSettings.ClockSkewMinutes),
                    RequireExpirationTime = true,
                    RequireSignedTokens = true
                };

                var principal = tokenHandler.ValidateToken(accessToken, validationParameters, out var validatedToken);
                return principal;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Verifica si un access token JWT ha expirado.
        /// </summary>
        /// <param name="accessToken">Access token JWT a verificar</param>
        /// <returns>true si el token ha expirado, false en caso contrario</returns>
        /// <remarks>
        /// Este método solo verifica la expiración, no valida otros aspectos del token.
        /// Para validación completa, use ValidateAccessToken.
        /// </remarks>
        public bool IsTokenExpired(string accessToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(accessToken);
                return jwtToken.ValidTo < DateTime.UtcNow;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
} 