using System;
using System.Threading.Tasks;
using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Application.Services;
using BackendSport.Domain.Entities.AuthEntities;

namespace BackendSport.Application.UseCases.AuthUseCases
{
    public class RefreshTokenUseCase
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;

        public RefreshTokenUseCase(IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository, ITokenService tokenService, IPasswordService passwordService)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _tokenService = tokenService;
            _passwordService = passwordService;
        }

        public async Task<RefreshTokenResponseDto?> ExecuteAsync(RefreshTokenRequestDto dto)
        {
            try
            {
                var parts = dto.RefreshToken.Split('.');
                if (parts.Length != 2)
                {
                    throw new InvalidOperationException("Formato de refresh token inválido");
                }
                var tokenId = parts[0];
                var tokenSecret = parts[1];

                var storedToken = await _refreshTokenRepository.GetByTokenIdAsync(tokenId);
                if (storedToken == null)
                {
                    throw new InvalidOperationException("Token no encontrado");
                }
                if (!_passwordService.VerifyPassword(tokenSecret, storedToken.TokenHash))
                {
                    throw new InvalidOperationException("Token inválido");
                }
                
                if (storedToken.ExpiresAt < DateTime.UtcNow)
                {
                    // Token expirado
                    throw new InvalidOperationException("Token expirado");
                }
                
                if (storedToken.Revoked)
                {
                    // Token revocado
                    throw new InvalidOperationException("Token revocado");
                }

                // Obtener el usuario
                var user = await _userRepository.GetByIdAsync(storedToken.UserId);
                if (user == null)
                {
                    // Usuario no encontrado
                    throw new InvalidOperationException("Usuario no encontrado");
                }

                // Generar nuevos tokens
                var newAccessToken = _tokenService.GenerateAccessToken(user);
                var newRefreshToken = _tokenService.GenerateRefreshToken(user);
                var newParts = newRefreshToken.Split('.');
                var newTokenId = newParts[0];
                var newTokenSecret = newParts[1];
                var newRefreshTokenHash = _passwordService.HashPassword(newTokenSecret);

                // Guardar el nuevo refresh token y revocar el anterior
                await _refreshTokenRepository.RevokeAsync(storedToken.Id);
                var newToken = new RefreshToken
                {
                    UserId = user.Id,
                    TokenId = newTokenId,
                    TokenHash = newRefreshTokenHash,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(7)
                };
                await _refreshTokenRepository.CreateAsync(newToken);

                return new RefreshTokenResponseDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                };
            }
            catch (Exception ex)
            {
                // Log del error para debugging
                throw new InvalidOperationException($"Error en RefreshTokenUseCase: {ex.Message}", ex);
            }
        }
    }
} 