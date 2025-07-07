using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Application.Interfaces.RolInterfaces;
using BackendSport.Application.Interfaces.PermisosInterfaces;
using System.Linq;
using System.Threading.Tasks;

namespace BackendSport.Application.Services
{
    public class PermissionCheckerService : IPermissionCheckerService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRolRepository _rolRepository;
        private readonly IPermisosRepository _permisosRepository;

        public PermissionCheckerService(
            IUserRepository userRepository,
            IRolRepository rolRepository,
            IPermisosRepository permisosRepository)
        {
            _userRepository = userRepository;
            _rolRepository = rolRepository;
            _permisosRepository = permisosRepository;
        }

        public async Task<bool> UserHasPermissionAsync(string userId, string permissionName)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.RolIds == null || !user.RolIds.Any())
                return false;

            foreach (var rolId in user.RolIds)
            {
                var rol = await _rolRepository.ObtenerPorIdAsync(rolId);
                if (rol == null || rol.Permisos == null) continue;
                foreach (var permisoId in rol.Permisos)
                {
                    var permiso = await _permisosRepository.ObtenerPorIdAsync(permisoId);
                    if (permiso != null && permiso.Nombre == permissionName)
                        return true;
                }
            }
            return false;
        }
    }
} 