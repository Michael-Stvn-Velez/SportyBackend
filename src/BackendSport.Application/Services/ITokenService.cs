using System.Security.Claims;
using BackendSport.Domain.Entities.AuthEntities;

namespace BackendSport.Application.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken(User user);
        string? GetUserIdFromAccessToken(string accessToken);
        ClaimsPrincipal? ValidateAccessToken(string accessToken);
        bool IsTokenExpired(string accessToken);
    }
} 