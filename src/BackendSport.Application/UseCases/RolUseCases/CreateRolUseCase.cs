using BackendSport.Application.DTOs.RolDTOs;
using BackendSport.Application.Interfaces.RolInterfaces;
using BackendSport.Domain.Entities.RolEntities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BackendSport.Application.UseCases.RolUseCases
{
    public class CreateRolUseCase
    {
        private readonly IRolRepository _rolRepository;

        public CreateRolUseCase(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task<RolDto> ExecuteAsync(RolDto rolDto)
        {
            if (await _rolRepository.ExisteNombreAsync(rolDto.Nombre))
                throw new Exception("Ya existe un rol con ese nombre.");

            var rol = new Rol
            {
                Id = Guid.NewGuid().ToString(),
                Nombre = rolDto.Nombre,
                Descripcion = rolDto.Descripcion,
                Permisos = new List<string>()
            };
            var creado = await _rolRepository.CrearRolAsync(rol);
            return new RolDto
            {
                Nombre = creado.Nombre,
                Descripcion = creado.Descripcion
            };
        }
    }
} 