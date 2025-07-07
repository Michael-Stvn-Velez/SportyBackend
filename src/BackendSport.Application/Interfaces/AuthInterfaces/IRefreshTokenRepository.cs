using BackendSport.Domain.Entities.AuthEntities;

namespace BackendSport.Application.Interfaces.AuthInterfaces
{
    public interface IRefreshTokenRepository
    {
        Task CreateAsync(RefreshToken token);
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);
        Task<IEnumerable<RefreshToken>> GetByUserIdAsync(string userId);
        Task RevokeAsync(string tokenId);
        Task DeleteAsync(string tokenId);
        Task<IEnumerable<RefreshToken>> GetAllActiveAsync();
        Task<RefreshToken?> GetByTokenIdAsync(string tokenId);
        Task RevokeAllByUserIdAsync(string userId);
    }
} 