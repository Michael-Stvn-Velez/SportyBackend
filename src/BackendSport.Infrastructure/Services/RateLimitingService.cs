using System.Collections.Concurrent;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Logging;

namespace BackendSport.Infrastructure.Services;

/// <summary>
/// Servicio para implementar rate limiting (limitación de velocidad) en la aplicación.
/// Proporciona control granular sobre las solicitudes por IP, usuario y endpoint.
/// </summary>
/// <remarks>
/// Este servicio utiliza FixedWindowRateLimiter de .NET para implementar rate limiting
/// con ventanas de tiempo fijas. Cada límite se mantiene en memoria usando un ConcurrentDictionary
/// para thread-safety.
/// </remarks>
public class RateLimitingService
{
    private readonly ConcurrentDictionary<string, RateLimiter> _rateLimiters = new();
    private readonly ILogger<RateLimitingService> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de rate limiting.
    /// </summary>
    /// <param name="logger">Logger para registrar eventos de rate limiting</param>
    public RateLimitingService(ILogger<RateLimitingService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Intenta adquirir un permiso de rate limiting para una clave específica.
    /// </summary>
    /// <param name="key">Clave única para identificar el límite (ej: "ip:192.168.1.1")</param>
    /// <param name="maxRequests">Número máximo de solicitudes permitidas</param>
    /// <param name="window">Ventana de tiempo para el límite</param>
    /// <returns>true si se adquirió el permiso, false si se excedió el límite</returns>
    /// <remarks>
    /// Si la clave no existe, se crea un nuevo RateLimiter. Si ya existe, se reutiliza.
    /// El rate limiter usa FixedWindowRateLimiter con QueueLimit = 0 para rechazar inmediatamente
    /// las solicitudes que excedan el límite.
    /// </remarks>
    public async Task<bool> TryAcquireAsync(string key, int maxRequests, TimeSpan window)
    {
        var limiter = _rateLimiters.GetOrAdd(key, _ => new FixedWindowRateLimiter(new FixedWindowRateLimiterOptions
        {
            PermitLimit = maxRequests,
            Window = window,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 0
        }));

        using var lease = await limiter.AcquireAsync();
        
        if (lease.IsAcquired)
        {
            _logger.LogDebug("Rate limit permit acquired for key: {Key}", key);
            return true;
        }

        _logger.LogWarning("Rate limit exceeded for key: {Key}", key);
        return false;
    }

    /// <summary>
    /// Intenta adquirir un permiso de rate limiting para un usuario específico.
    /// </summary>
    /// <param name="userId">ID del usuario</param>
    /// <param name="maxRequests">Número máximo de solicitudes permitidas</param>
    /// <param name="window">Ventana de tiempo para el límite</param>
    /// <returns>true si se adquirió el permiso, false si se excedió el límite</returns>
    /// <remarks>
    /// La clave se genera como "user:{userId}" para evitar conflictos con otros tipos de límites.
    /// </remarks>
    public async Task<bool> TryAcquireForUserAsync(string userId, int maxRequests, TimeSpan window)
    {
        return await TryAcquireAsync($"user:{userId}", maxRequests, window);
    }

    /// <summary>
    /// Intenta adquirir un permiso de rate limiting para una dirección IP específica.
    /// </summary>
    /// <param name="ipAddress">Dirección IP del cliente</param>
    /// <param name="maxRequests">Número máximo de solicitudes permitidas</param>
    /// <param name="window">Ventana de tiempo para el límite</param>
    /// <returns>true si se adquirió el permiso, false si se excedió el límite</returns>
    /// <remarks>
    /// La clave se genera como "ip:{ipAddress}" para evitar conflictos con otros tipos de límites.
    /// </remarks>
    public async Task<bool> TryAcquireForIpAsync(string ipAddress, int maxRequests, TimeSpan window)
    {
        return await TryAcquireAsync($"ip:{ipAddress}", maxRequests, window);
    }

    /// <summary>
    /// Intenta adquirir un permiso de rate limiting para un endpoint específico.
    /// </summary>
    /// <param name="endpoint">Ruta del endpoint (ej: "/api/user/login")</param>
    /// <param name="identifier">Identificador adicional (ej: IP o userId)</param>
    /// <param name="maxRequests">Número máximo de solicitudes permitidas</param>
    /// <param name="window">Ventana de tiempo para el límite</param>
    /// <returns>true si se adquirió el permiso, false si se excedió el límite</returns>
    /// <remarks>
    /// La clave se genera como "endpoint:{endpoint}:{identifier}" para permitir límites
    /// específicos por endpoint y cliente.
    /// </remarks>
    public async Task<bool> TryAcquireForEndpointAsync(string endpoint, string identifier, int maxRequests, TimeSpan window)
    {
        return await TryAcquireAsync($"endpoint:{endpoint}:{identifier}", maxRequests, window);
    }
} 