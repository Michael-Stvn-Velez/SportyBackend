using BackendSport.Application.DTOs.DeporteDTOs;
using BackendSport.Application.Interfaces.DeporteInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendSport.Application.UseCases.DeporteUseCases
{
    public class GetAllDeportesUseCase
    {
        private readonly IDeporteRepository _repository;
        public GetAllDeportesUseCase(IDeporteRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DeporteListDto>> ExecuteAsync()
        {
            var deportes = await _repository.GetAllAsync();
            if (deportes == null)
                throw new Exception("No se encontraron deportes.");
            return deportes.Select(d => new DeporteListDto
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
            }).ToList();
        }
    }
} 