using BackendSport.Application.DTOs.RolDTOs;
using BackendSport.Application.Interfaces.RolInterfaces;
using BackendSport.Domain.Entities.RolEntities;
using System.Threading.Tasks;

namespace BackendSport.Application.UseCases.RolUseCases
{
    public class UpdateRolUseCase
    {
        private readonly IRolRepository _rolRepository;

        public UpdateRolUseCase(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task<bool> ExecuteAsync(string id, RolUpdateDto rolUpdateDto)
        {
            // Buscar el rol existente
            var rolExistente = await _rolRepository.ObtenerPorIdAsync(id);
            if (rolExistente == null)
                throw new System.Exception("Rol no encontrado.");

            // Validar si el nuevo nombre ya existe en otro rol
            if (rolExistente.Nombre != rolUpdateDto.Nombre && await _rolRepository.ExisteNombreAsync(rolUpdateDto.Nombre))
                throw new System.Exception("Ya existe un rol con ese nombre.");

            rolExistente.Nombre = rolUpdateDto.Nombre;
            rolExistente.Descripcion = rolUpdateDto.Descripcion;
            await _rolRepository.ActualizarRolAsync(rolExistente);
            return true;
        }
    }
} 