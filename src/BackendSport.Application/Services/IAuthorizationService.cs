namespace BackendSport.Application.Services
{
    public interface IAuthorizationService
    {
        Task<bool> ValidatePermissionAsync(string accessToken, string permission);
        string? GetUserIdFromTokenAsync(string accessToken);
    }
} 