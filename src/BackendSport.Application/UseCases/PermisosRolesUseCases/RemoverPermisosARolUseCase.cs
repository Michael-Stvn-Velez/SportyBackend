using BackendSport.Application.DTOs.PermisosRolesDTOs;
using BackendSport.Application.Interfaces.RolInterfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BackendSport.Application.UseCases.PermisosRolesUseCases
{
    public class RemoverPermisosARolUseCase
    {
        private readonly IRolRepository _rolRepository;

        public RemoverPermisosARolUseCase(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task<bool> ExecuteAsync(string rolId, RemoverPermisosRolDto dto)
        {
            var rol = await _rolRepository.ObtenerPorIdAsync(rolId);
            if (rol == null)
                throw new Exception("Rol no encontrado.");

            bool removed = false;
            foreach (var permisoId in dto.PermisosIds.Distinct())
            {
                if (rol.Permisos.Contains(permisoId))
                {
                    rol.Permisos.Remove(permisoId);
                    removed = true;
                }
            }

            if (removed)
                await _rolRepository.ActualizarRolAsync(rol);

            return removed;
        }
    }
} 