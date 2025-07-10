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
            DeporteName = deporte.Name,
            Positions = deporte.Positions ?? new List<string>(),
            Levels = deporte.CompetitiveLevel ?? new List<string>(),
            PerformanceMetrics = deporte.PerformanceMetrics ?? new List<string>()
        };
    }
} 