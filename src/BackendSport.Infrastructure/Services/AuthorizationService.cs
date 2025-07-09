using BackendSport.Application.Services;
using BackendSport.Infrastructure.Services;

namespace BackendSport.Infrastructure.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ITokenService _tokenService;
        private readonly IPermissionCheckerService _permissionCheckerService;

        public AuthorizationService(ITokenService tokenService, IPermissionCheckerService permissionCheckerService)
        {
            _tokenService = tokenService;
            _permissionCheckerService = permissionCheckerService;
        }

        public async Task<bool> ValidatePermissionAsync(string accessToken, string permission)
        {
            var userId = GetUserIdFromTokenAsync(accessToken);
            if (string.IsNullOrEmpty(userId))
                return false;

            return await _permissionCheckerService.UserHasPermissionAsync(userId, permission);
        }

        public string? GetUserIdFromTokenAsync(string accessToken)
        {
            return _tokenService.GetUserIdFromAccessToken(accessToken);
        }
    }
} 