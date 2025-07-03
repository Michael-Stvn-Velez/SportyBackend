using BackendSport.Application.Interfaces.PermisosInterfaces;
using System.Threading.Tasks;

namespace BackendSport.Application.UseCases.PermisosUseCases
{
    public class DeletePermisosUseCase
    {
        private readonly IPermisosRepository _permisosRepository;

        public DeletePermisosUseCase(IPermisosRepository permisosRepository)
        {
            _permisosRepository = permisosRepository;
        }

        public async Task<bool> ExecuteAsync(string id)
        {
            var permisos = await _permisosRepository.ObtenerPorIdAsync(id);
            if (permisos == null)
                throw new System.Exception("Permisos no encontrado.");
            await _permisosRepository.EliminarPermisosAsync(id);
            return true;
        }
    }
} 