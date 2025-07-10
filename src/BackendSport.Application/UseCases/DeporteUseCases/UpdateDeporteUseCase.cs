using BackendSport.Application.DTOs.DeporteDTOs;
using BackendSport.Application.Interfaces.DeporteInterfaces;
using BackendSport.Domain.Entities.DeporteEntities;
using System.Threading.Tasks;

namespace BackendSport.Application.UseCases.DeporteUseCases
{
    public class UpdateDeporteUseCase
    {
        private readonly IDeporteRepository _repository;
        public UpdateDeporteUseCase(IDeporteRepository repository)
        {
            _repository = repository;
        }

        public async Task<DeporteDto> ExecuteAsync(string id, DeporteUpdateDto dto)
        {
            var existente = await _repository.GetByIdAsync(id);
            if (existente == null) 
                throw new Exception("No se encontró el deporte.");
            if (existente.Name != dto.Name && await _repository.ExistsByNameAsync(dto.Name))
                throw new Exception("Ya existe un deporte con ese nombre.");

            existente.Name = dto.Name;
            existente.Modalities = dto.Modalities ?? new List<string>();
            existente.Surfaces = dto.Surfaces ?? new List<string>();
            existente.Positions = dto.Positions ?? new List<string>();
            existente.Statistics = dto.Statistics ?? new List<string>();
            existente.PerformanceMetrics = dto.PerformanceMetrics ?? new List<string>();
            existente.EvaluationTypes = dto.EvaluationTypes ?? new List<string>();
            existente.Formations = dto.Formations ?? new List<string>();
            existente.CompetitiveLevel = dto.CompetitiveLevel ?? new List<string>();

            await _repository.UpdateAsync(id, existente);
            return new DeporteDto
            {
                Name = existente.Name,
                Modalities = existente.Modalities,
                Surfaces = existente.Surfaces,
                Positions = existente.Positions,
                Statistics = existente.Statistics,
                PerformanceMetrics = existente.PerformanceMetrics,
                EvaluationTypes = existente.EvaluationTypes,
                Formations = existente.Formations,
                CompetitiveLevel = existente.CompetitiveLevel
            };
        }
    }
} 