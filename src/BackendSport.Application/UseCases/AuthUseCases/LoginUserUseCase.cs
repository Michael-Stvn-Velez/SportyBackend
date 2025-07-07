using System.Threading.Tasks;
using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Application.Services;
using BackendSport.Domain.Entities.AuthEntities;

namespace BackendSport.Application.UseCases.AuthUseCases
{
    public class LoginUserUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LoginUserUseCase(IUserRepository userRepository, IPasswordService passwordService, ITokenService tokenService, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<LoginResponseDto> ExecuteAsync(LoginRequestDto loginDto)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(loginDto.Email);
                if (user == null || !_passwordService.VerifyPassword(loginDto.Password, user.Password))
                {
                    throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");
                }

                var accessToken = _tokenService.GenerateAccessToken(user);
                var refreshToken = _tokenService.GenerateRefreshToken(user);
                var parts = refreshToken.Split('.');
                var tokenId = parts[0];
                var tokenSecret = parts[1];
                var refreshTokenHash = _passwordService.HashPassword(tokenSecret);

                var refreshTokenEntity = new RefreshToken
                {
                    UserId = user.Id,
                    TokenId = tokenId,
                    TokenHash = refreshTokenHash,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                };
                
                await _refreshTokenRepository.CreateAsync(refreshTokenEntity);

                return new LoginResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error en LoginUserUseCase: {ex.Message}", ex);
            }
        }
    }
} 