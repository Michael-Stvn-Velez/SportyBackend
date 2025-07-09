using BackendSport.Application.DTOs.DeporteDTOs;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Application.Interfaces.DeporteInterfaces;

namespace BackendSport.Application.UseCases.AuthUseCases;

public class ConfigureUserSportUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IDeporteRepository _deporteRepository;

    public ConfigureUserSportUseCase(IUserRepository userRepository, IDeporteRepository deporteRepository)
    {
        _userRepository = userRepository;
        _deporteRepository = deporteRepository;
    }

    public async Task ExecuteAsync(string userId, string sportId, ConfigureUserSportDto dto)
    {
        // Validar que el usuario existe
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("Usuario no encontrado");
        }

        // Validar que el deporte existe
        var deporte = await _deporteRepository.GetByIdAsync(sportId);
        if (deporte == null)
        {
            throw new InvalidOperationException("Deporte no encontrado");
        }

        // Validar que el usuario tiene el deporte asignado
        if (!user.HasSport(sportId))
        {
            throw new InvalidOperationException("El usuario no tiene este deporte asignado");
        }

        // Validar que no esté ya configurado
        if (user.IsSportConfigured(sportId))
        {
            throw new InvalidOperationException("Este deporte ya está configurado para el usuario");
        }

        // Validar posiciones (máximo 3)
        if (dto.Positions.Count > 3)
        {
            throw new InvalidOperationException("Máximo 3 posiciones permitidas");
        }

        // Validar que las posiciones existan en el deporte
        var posicionesValidas = deporte.Posiciones ?? new List<string>();
        if (dto.Positions.Any(p => !posicionesValidas.Contains(p)))
        {
            throw new InvalidOperationException("Una o más posiciones no son válidas para este deporte");
        }

        // Validar que el nivel sea válido
        var nivelesValidos = deporte.NivelCompetitivo ?? new List<string>();
        if (!nivelesValidos.Contains(dto.Level))
        {
            throw new InvalidOperationException("El nivel competitivo no es válido para este deporte");
        }

        // Validar métricas de rendimiento
        var metricasValidas = deporte.MetricasRendimiento ?? new List<string>();
        if (dto.PerformanceMetrics.Any(m => !metricasValidas.Contains(m.Key)))
        {
            throw new InvalidOperationException("Una o más métricas de rendimiento no son válidas para este deporte");
        }

        // Validar rangos de valores (1-10)
        if (dto.PerformanceMetrics.Any(m => m.Value < 1 || m.Value > 10))
        {
            throw new InvalidOperationException("Los valores de métricas deben estar entre 1 y 10");
        }

        // Configurar el deporte
        var success = user.ConfigureSport(sportId, dto.Positions, dto.Level, dto.PerformanceMetrics);
        if (!success)
        {
            throw new InvalidOperationException("No se pudo configurar el deporte");
        }

        // Guardar en la base de datos
        await _userRepository.UpdateAsync(user);
    }
} 