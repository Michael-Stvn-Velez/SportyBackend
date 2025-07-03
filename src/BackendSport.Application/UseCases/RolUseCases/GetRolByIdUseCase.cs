using BackendSport.Application.DTOs.RolDTOs;
using BackendSport.Application.Interfaces.RolInterfaces;
using System.Threading.Tasks;

namespace BackendSport.Application.UseCases.RolUseCases
{
    public class GetRolByIdUseCase
    {
        private readonly IRolRepository _rolRepository;

        public GetRolByIdUseCase(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task<RolListDto?> ExecuteAsync(string id)
        {
            var rol = await _rolRepository.ObtenerPorIdAsync(id);
            if (rol == null) return null;
            return new RolListDto
            {
                Id = rol.Id,
                Nombre = rol.Nombre,
                Descripcion = rol.Descripcion,
                Permisos = rol.Permisos
            };
        }
    }
} 