using System;
using System.Threading.Tasks;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Application.Services;

namespace BackendSport.Application.UseCases.AuthUseCases
{
    public class LogoutAllUserDevicesUseCase
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;

        public LogoutAllUserDevicesUseCase(IRefreshTokenRepository refreshTokenRepository, ITokenService tokenService)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
        }

        public async Task ExecuteAsync(string accessToken)
        {
            var userId = _tokenService.GetUserIdFromAccessToken(accessToken);
            if (string.IsNullOrEmpty(userId))
                throw new InvalidOperationException("No se pudo extraer el userId del access token");

            await _refreshTokenRepository.RevokeAllByUserIdAsync(userId);
        }
    }
} 