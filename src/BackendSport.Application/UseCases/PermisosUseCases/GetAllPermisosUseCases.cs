using BackendSport.Application.DTOs.PermisosDTOs;
using BackendSport.Application.Interfaces.PermisosInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendSport.Application.UseCases.PermisosUseCases
{
    public class GetAllPermisosUseCase
    {
        private readonly IPermisosRepository _permisosRepository;

        public GetAllPermisosUseCase(IPermisosRepository permisosRepository)
        {
            _permisosRepository = permisosRepository;
        }

        public async Task<List<PermisosListDto>> ExecuteAsync()
        {
            var permisos = await _permisosRepository.ObtenerTodosAsync();
            return permisos.Select(p => new PermisosListDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
            }).ToList();
        }
    }
} 