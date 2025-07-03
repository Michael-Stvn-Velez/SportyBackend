using BackendSport.Application.DTOs.PermisosDTOs;
using BackendSport.Application.Interfaces.PermisosInterfaces;
using System.Threading.Tasks;

namespace BackendSport.Application.UseCases.PermisosUseCases
{
    public class UpdatePermisosUseCase
    {
        private readonly IPermisosRepository _permisosRepository;

        public UpdatePermisosUseCase(IPermisosRepository permisosRepository)
        {
            _permisosRepository = permisosRepository;
        }

        public async Task<bool> ExecuteAsync(string id, PermisosUpdateDto permisosUpdateDto)
        {
            // Buscar el permisos existente
            var permisosExistente = await _permisosRepository.ObtenerPorIdAsync(id);
            if (permisosExistente == null)
                throw new System.Exception("Permisos no encontrado.");

            // Validar si el nuevo nombre ya existe en otro permisos
            if (permisosExistente.Nombre != permisosUpdateDto.Nombre && await _permisosRepository.ExisteNombreAsync(permisosUpdateDto.Nombre))
                throw new System.Exception("Ya existe un permisos con ese nombre.");

            permisosExistente.Nombre = permisosUpdateDto.Nombre;
            permisosExistente.Descripcion = permisosUpdateDto.Descripcion;
            await _permisosRepository.ActualizarPermisosAsync(permisosExistente);
            return true;
        }
    }
} 