using BackendSport.Application.Interfaces.DeporteInterfaces;
using System.Threading.Tasks;

namespace BackendSport.Application.UseCases.DeporteUseCases
{
    public class DeleteDeporteUseCase
    {
        private readonly IDeporteRepository _repository;
        public DeleteDeporteUseCase(IDeporteRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ExecuteAsync(string id)
        {
            var existente = await _repository.GetByIdAsync(id);
            if (existente == null) 
                throw new Exception("No se encontró el deporte.");
            await _repository.DeleteAsync(id);
            return true;
        }
    }
} 