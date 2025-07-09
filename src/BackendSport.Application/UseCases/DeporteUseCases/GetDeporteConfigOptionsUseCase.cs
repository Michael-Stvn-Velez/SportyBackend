using BackendSport.Application.DTOs.DeporteDTOs;
using BackendSport.Application.Interfaces.DeporteInterfaces;

namespace BackendSport.Application.UseCases.DeporteUseCases;

public class GetDeporteConfigOptionsUseCase
{
    private readonly IDeporteRepository _deporteRepository;

    public GetDeporteConfigOptionsUseCase(IDeporteRepository deporteRepository)
    {
        _deporteRepository = deporteRepository;
    }

    public async Task<DeporteConfigOptionsDto> ExecuteAsync(string deporteId)
    {
        var deporte = await _deporteRepository.GetByIdAsync(deporteId);
        if (deporte == null)
        {
            throw new InvalidOperationException("Deporte no encontrado");
        }

        return new DeporteConfigOptionsDto
        {
            DeporteId = deporte.Id,
            DeporteNombre = deporte.Nombre,
            Posiciones = deporte.Posiciones ?? new List<string>(),
            Niveles = deporte.NivelCompetitivo ?? new List<string>(),
            MetricasRendimiento = deporte.MetricasRendimiento ?? new List<string>()
        };
    }
} 