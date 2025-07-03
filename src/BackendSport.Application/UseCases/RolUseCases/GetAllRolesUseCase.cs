using BackendSport.Application.DTOs.RolDTOs;
using BackendSport.Application.Interfaces.RolInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendSport.Application.UseCases.RolUseCases
{
    public class GetAllRolesUseCase
    {
        private readonly IRolRepository _rolRepository;

        public GetAllRolesUseCase(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task<List<RolListDto>> ExecuteAsync()
        {
            var roles = await _rolRepository.ObtenerTodosAsync();
            return roles.Select(r => new RolListDto
            {
                Id = r.Id,
                Nombre = r.Nombre,
                Descripcion = r.Descripcion,
                Permisos = r.Permisos
            }).ToList();
        }
    }
} 