using BackendSport.Application.DTOs.DeporteDTOs;
using BackendSport.Application.Interfaces.DeporteInterfaces;
using System.Threading.Tasks;

namespace BackendSport.Application.UseCases.DeporteUseCases
{
    public class GetDeporteByIdUseCase
    {
        private readonly IDeporteRepository _repository;
        public GetDeporteByIdUseCase(IDeporteRepository repository)
        {
            _repository = repository;
        }

        public async Task<DeporteListDto?> ExecuteAsync(string id)
        {
            var d = await _repository.GetByIdAsync(id);
            if (d == null) 
                throw new Exception("No se encontró el deporte.");
                
            return new DeporteListDto
            {
                Id = d.Id,
                Name = d.Name,
                Modalities = d.Modalities,
                Surfaces = d.Surfaces,
                Positions = d.Positions,
                Statistics = d.Statistics,
                PerformanceMetrics = d.PerformanceMetrics,
                EvaluationTypes = d.EvaluationTypes,
                Formations = d.Formations,
                CompetitiveLevel = d.CompetitiveLevel
            };
        }
    }
} 