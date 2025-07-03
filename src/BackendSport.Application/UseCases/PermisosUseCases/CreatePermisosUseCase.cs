using BackendSport.Application.DTOs.PermisosDTOs;
using BackendSport.Application.Interfaces.PermisosInterfaces;
using BackendSport.Domain.Entities.PermisosEntities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BackendSport.Application.UseCases.PermisosUseCases
{
    public class CreatePermisosUseCase
    {
        private readonly IPermisosRepository _permisosRepository;

        public CreatePermisosUseCase(IPermisosRepository permisosRepository)
        {
            _permisosRepository = permisosRepository;
        }

        public async Task<PermisosDto> ExecuteAsync(PermisosDto permisosDto)
        {
            if (await _permisosRepository.ExisteNombreAsync(permisosDto.Nombre))
                throw new Exception("Ya existe un permisos con ese nombre.");

            var permisos = new Permisos
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = permisosDto.Nombre,
                Descripcion = permisosDto.Descripcion,
            };
            var creado = await _permisosRepository.CrearPermisosAsync(permisos);
            return new PermisosDto
            {
                Nombre = creado.Nombre,
                Descripcion = creado.Descripcion
            };
        }
    }
} 