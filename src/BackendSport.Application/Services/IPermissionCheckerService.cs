namespace BackendSport.Application.Services
{
    public interface IPermissionCheckerService
    {
        Task<bool> UserHasPermissionAsync(string userId, string permissionName);
    }
} 