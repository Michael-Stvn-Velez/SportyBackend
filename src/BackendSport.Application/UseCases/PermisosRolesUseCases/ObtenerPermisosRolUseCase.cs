using BackendSport.Application.DTOs.PermisosRolesDTOs;
using BackendSport.Application.Interfaces.PermisosInterfaces;
using BackendSport.Application.Interfaces.RolInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendSport.Application.UseCases.PermisosRolesUseCases
{
    public class ObtenerPermisosRolUseCase
    {
        private readonly IRolRepository _rolRepository;
        private readonly IPermisosRepository _permisosRepository;

        public ObtenerPermisosRolUseCase(IRolRepository rolRepository, IPermisosRepository permisosRepository)
        {
            _rolRepository = rolRepository;
            _permisosRepository = permisosRepository;
        }

        public async Task<List<PermisosRolDto>> ExecuteAsync(string rolId)
        {
            var rol = await _rolRepository.ObtenerPorIdAsync(rolId);
            if (rol == null)
                throw new Exception("Rol no encontrado.");

            if (rol.Permisos == null || !rol.Permisos.Any())
                return new List<PermisosRolDto>();

            var permisos = new List<PermisosRolDto>();
            foreach (var permisoId in rol.Permisos)
            {
                var permiso = await _permisosRepository.ObtenerPorIdAsync(permisoId);
                if (permiso != null)
                {
                    permisos.Add(new PermisosRolDto
                    {
                        Id = permiso.Id,
                        Nombre = permiso.Nombre,
                        Descripcion = permiso.Descripcion
                    });
                }
            }

            return permisos;
        }
    }
} 