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
                Nombre = d.Nombre,
                Modalidad = d.Modalidad,
                Superficie = d.Superficie,
                Posiciones = d.Posiciones,
                Estadisticas = d.Estadisticas,
                MetricasRendimiento = d.MetricasRendimiento,
                TipoEvaluaciones = d.TipoEvaluaciones,
                Formaciones = d.Formaciones,
                NivelCompetitivo = d.NivelCompetitivo
            };
        }
    }
} 