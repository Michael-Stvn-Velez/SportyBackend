using BackendSport.Application.DTOs.DeporteDTOs;
using BackendSport.Application.Interfaces.DeporteInterfaces;
using BackendSport.Domain.Entities.DeporteEntities;
using System.Threading.Tasks;

namespace BackendSport.Application.UseCases.DeporteUseCases
{
    public class CreateDeporteUseCase
    {
        private readonly IDeporteRepository _repository;
        public CreateDeporteUseCase(IDeporteRepository repository)
        {
            _repository = repository;
        }

        public async Task<DeporteListDto> ExecuteAsync(DeporteDto dto)
        {
            if (await _repository.ExistsByNameAsync(dto.Name))
                throw new Exception("Ya existe un deporte con ese nombre.");

            var deporte = new Deporte
            {
                Id = System.Guid.NewGuid().ToString(),
                Name = dto.Name,
                Modalities = dto.Modalities ?? new List<string>(),
                Surfaces = dto.Surfaces ?? new List<string>(),
                Positions = dto.Positions ?? new List<string>(),
                Statistics = dto.Statistics ?? new List<string>(),
                PerformanceMetrics = dto.PerformanceMetrics ?? new List<string>(),
                EvaluationTypes = dto.EvaluationTypes ?? new List<string>(),
                Formations = dto.Formations ?? new List<string>(),
                CompetitiveLevel = dto.CompetitiveLevel ?? new List<string>()
            };
            
            await _repository.AddAsync(deporte);
            
            return new DeporteListDto
            {
                Id = deporte.Id,
                Name = deporte.Name,
                Modalities = deporte.Modalities,
                Surfaces = deporte.Surfaces,
                Positions = deporte.Positions,
                Statistics = deporte.Statistics,
                PerformanceMetrics = deporte.PerformanceMetrics,
                EvaluationTypes = deporte.EvaluationTypes,
                Formations = deporte.Formations,
                CompetitiveLevel = deporte.CompetitiveLevel
            };
        }
    }
} 