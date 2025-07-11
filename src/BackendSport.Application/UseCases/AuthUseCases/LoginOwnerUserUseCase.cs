using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Application.Services;
using BackendSport.Domain.Entities.AuthEntities;

namespace BackendSport.Application.UseCases.AuthUseCases;

public class LoginOwnerUserUseCase
{
    private readonly IOwnerUserRepository _ownerUserRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public LoginOwnerUserUseCase(IOwnerUserRepository ownerUserRepository, IPasswordService passwordService, ITokenService tokenService)
    {
        _ownerUserRepository = ownerUserRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto> ExecuteAsync(LoginRequestDto loginDto)
    {
        var ownerUser = await _ownerUserRepository.GetByEmailAsync(loginDto.Email);
        if (ownerUser == null || !_passwordService.VerifyPassword(loginDto.Password, ownerUser.Password))
        {
            throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");
        }

        // Crear el access token solo con Email, Name y RolIds
        var accessToken = _tokenService.GenerateAccessToken(new User {
            Email = ownerUser.Email,
            Name = ownerUser.Name,
            RolIds = ownerUser.RolIds
        });
        var refreshToken = _tokenService.GenerateRefreshToken(new User {
            Email = ownerUser.Email,
            Name = ownerUser.Name,
            RolIds = ownerUser.RolIds
        });

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}