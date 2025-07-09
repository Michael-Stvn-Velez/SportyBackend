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
            if (existente.Nombre != dto.Nombre && await _repository.ExistsByNombreAsync(dto.Nombre))
                throw new Exception("Ya existe un deporte con ese nombre.");

            existente.Nombre = dto.Nombre;
            existente.Modalidad = dto.Modalidad ?? new List<string>();
            existente.Superficie = dto.Superficie ?? new List<string>();
            existente.Posiciones = dto.Posiciones ?? new List<string>();
            existente.Estadisticas = dto.Estadisticas ?? new List<string>();
            existente.MetricasRendimiento = dto.MetricasRendimiento ?? new List<string>();
            existente.TipoEvaluaciones = dto.TipoEvaluaciones ?? new List<string>();
            existente.Formaciones = dto.Formaciones ?? new List<string>();
            existente.NivelCompetitivo = dto.NivelCompetitivo ?? new List<string>();

            await _repository.UpdateAsync(id, existente);
            return new DeporteDto
            {
                Nombre = existente.Nombre,
                Modalidad = existente.Modalidad,
                Superficie = existente.Superficie,
                Posiciones = existente.Posiciones,
                Estadisticas = existente.Estadisticas,
                MetricasRendimiento = existente.MetricasRendimiento,
                TipoEvaluaciones = existente.TipoEvaluaciones,
                Formaciones = existente.Formaciones,
                NivelCompetitivo = existente.NivelCompetitivo
            };
        }
    }
} 