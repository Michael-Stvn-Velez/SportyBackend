using System;
using System.Threading.Tasks;
using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.Interfaces.AuthInterfaces;

namespace BackendSport.Application.UseCases.AuthUseCases
{
    public class LogoutUserUseCase
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LogoutUserUseCase(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task ExecuteAsync(RefreshTokenRequestDto dto)
        {
            var parts = dto.RefreshToken.Split('.');
            if (parts.Length != 2)
                throw new InvalidOperationException("Formato de refresh token inválido");

            var tokenId = parts[0];
            var storedToken = await _refreshTokenRepository.GetByTokenIdAsync(tokenId);
            if (storedToken == null)
                throw new InvalidOperationException("Token no encontrado");

            await _refreshTokenRepository.RevokeAsync(storedToken.Id);
        }
    }
} 