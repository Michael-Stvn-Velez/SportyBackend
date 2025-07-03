using BackendSport.Application.DTOs.PermisosRolesDTOs;
using BackendSport.Application.Interfaces.PermisosInterfaces;
using BackendSport.Application.Interfaces.RolInterfaces;
using BackendSport.Domain.Entities.RolEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendSport.Application.UseCases.PermisosRolesUseCases
{
    public class AsignarPermisosARolUseCase
    {
        private readonly IRolRepository _rolRepository;
        private readonly IPermisosRepository _permisosRepository;

        public AsignarPermisosARolUseCase(IRolRepository rolRepository, IPermisosRepository permisosRepository)
        {
            _rolRepository = rolRepository;
            _permisosRepository = permisosRepository;
        }

        public async Task<bool> ExecuteAsync(string rolId, AsignarPermisosRolDto dto)
        {
            var rol = await _rolRepository.ObtenerPorIdAsync(rolId);
            if (rol == null)
                throw new Exception("Rol no encontrado.");

            foreach (var permisoId in dto.PermisosIds.Distinct())
            {
                if (rol.Permisos.Contains(permisoId))
                    continue; // Ya está asignado

                var permiso = await _permisosRepository.ObtenerPorIdAsync(permisoId);
                if (permiso == null)
                    throw new Exception($"Permiso con ID {permisoId} no existe.");

                rol.Permisos.Add(permisoId);
            }

            await _rolRepository.ActualizarRolAsync(rol);
            return true;
        }
    }
} 