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

        public async Task<DeporteDto> ExecuteAsync(DeporteDto dto)
        {
            if (await _repository.ExistsByNombreAsync(dto.Nombre))
                throw new Exception("Ya existe un deporte con ese nombre.");

            var deporte = new Deporte
            {
                Id = System.Guid.NewGuid().ToString(),
                Nombre = dto.Nombre,
                Modalidad = dto.Modalidad ?? new List<string>(),
                Superficie = dto.Superficie ?? new List<string>(),
                Posiciones = dto.Posiciones ?? new List<string>(),
                Estadisticas = dto.Estadisticas ?? new List<string>(),
                MetricasRendimiento = dto.MetricasRendimiento ?? new List<string>(),
                TipoEvaluaciones = dto.TipoEvaluaciones ?? new List<string>(),
                Formaciones = dto.Formaciones ?? new List<string>(),
                NivelCompetitivo = dto.NivelCompetitivo ?? new List<string>()
            };
            await _repository.AddAsync(deporte);
            return new DeporteDto
            {
                Nombre = deporte.Nombre,
                Modalidad = deporte.Modalidad,
                Superficie = deporte.Superficie,
                Posiciones = deporte.Posiciones,
                Estadisticas = deporte.Estadisticas,
                MetricasRendimiento = deporte.MetricasRendimiento,
                TipoEvaluaciones = deporte.TipoEvaluaciones,
                Formaciones = deporte.Formaciones,
                NivelCompetitivo = deporte.NivelCompetitivo
            };
        }
    }
} 