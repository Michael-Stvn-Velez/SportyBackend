using BackendSport.Application.DTOs.PermisosDTOs;
using BackendSport.Application.Interfaces.PermisosInterfaces;
using System.Threading.Tasks;

namespace BackendSport.Application.UseCases.PermisosUseCases
{
    public class GetPermisosByIdUseCase
    {
        private readonly IPermisosRepository _permisosRepository;

        public GetPermisosByIdUseCase(IPermisosRepository permisosRepository)
        {
            _permisosRepository = permisosRepository;
        }

        public async Task<PermisosListDto?> ExecuteAsync(string id)
        {
            var permisos = await _permisosRepository.ObtenerPorIdAsync(id);
            if (permisos == null) return null;
            return new PermisosListDto
            {
                Id = permisos.Id,
                Nombre = permisos.Nombre,
                Descripcion = permisos.Descripcion,
            };
        }
    }
} 