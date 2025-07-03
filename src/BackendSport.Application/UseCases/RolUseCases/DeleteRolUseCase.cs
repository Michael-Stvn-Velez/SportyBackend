using BackendSport.Application.Interfaces.RolInterfaces;
using System.Threading.Tasks;

namespace BackendSport.Application.UseCases.RolUseCases
{
    public class DeleteRolUseCase
    {
        private readonly IRolRepository _rolRepository;

        public DeleteRolUseCase(IRolRepository rolRepository)
        {
            _rolRepository = rolRepository;
        }

        public async Task<bool> ExecuteAsync(string id)
        {
            var rol = await _rolRepository.ObtenerPorIdAsync(id);
            if (rol == null)
                throw new System.Exception("Rol no encontrado.");
            await _rolRepository.EliminarRolAsync(id);
            return true;
        }
    }
} 