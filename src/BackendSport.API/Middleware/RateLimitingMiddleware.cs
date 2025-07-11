using BackendSport.Infrastructure.Services;
using System.Net;

namespace BackendSport.API.Middleware;

/// <summary>
/// Middleware para implementar rate limiting en las solicitudes HTTP.
/// Aplica límites de velocidad por IP, usuario y endpoint específico.
/// </summary>
/// <remarks>
/// Este middleware se ejecuta temprano en el pipeline para interceptar solicitudes
/// antes de que lleguen a los controladores. Implementa tres niveles de rate limiting:
/// 1. Por IP: Límite global por dirección IP
/// 2. Por endpoint: Límites específicos por ruta de endpoint
/// 3. Por usuario: Límites para usuarios autenticados
/// </remarks>
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly RateLimitingService _rateLimitingService;
    private readonly ILogger<RateLimitingMiddleware> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del middleware de rate limiting.
    /// </summary>
    /// <param name="next">Siguiente middleware en el pipeline</param>
    /// <param name="rateLimitingService">Servicio de rate limiting</param>
    /// <param name="logger">Logger para registrar eventos</param>
    public RateLimitingMiddleware(
        RequestDelegate next,
        RateLimitingService rateLimitingService,
        ILogger<RateLimitingMiddleware> logger)
    {
        _next = next;
        _rateLimitingService = rateLimitingService;
        _logger = logger;
    }

    /// <summary>
    /// Procesa la solicitud HTTP aplicando rate limiting.
    /// </summary>
    /// <param name="context">Contexto de la solicitud HTTP</param>
    /// <returns>Task que representa la operación asíncrona</returns>
    /// <remarks>
    /// El método aplica rate limiting en el siguiente orden:
    /// 1. Verifica límites por IP (100 requests/minuto)
    /// 2. Verifica límites por endpoint específico
    /// 3. Verifica límites por usuario (si está autenticado)
    /// 
    /// Si se excede cualquier límite, retorna HTTP 429 (Too Many Requests)
    /// con headers apropiados de rate limiting.
    /// </remarks>
    public async Task InvokeAsync(HttpContext context)
    {
        var ipAddress = GetClientIpAddress(context);
        var endpoint = context.Request.Path.Value ?? string.Empty;
        var userId = GetUserIdFromContext(context);

        // Rate limiting por IP
        var ipAllowed = await _rateLimitingService.TryAcquireForIpAsync(
            ipAddress, 
            maxRequests: 100, 
            window: TimeSpan.FromMinutes(1));

        if (!ipAllowed)
        {
            _logger.LogWarning("Rate limit exceeded for IP: {IpAddress}", ipAddress);
            await ReturnRateLimitExceeded(context, "Too many requests from this IP");
            return;
        }

        // Rate limiting por endpoint específico
        var endpointAllowed = await _rateLimitingService.TryAcquireForEndpointAsync(
            endpoint,
            ipAddress,
            maxRequests: GetMaxRequestsForEndpoint(endpoint),
            window: TimeSpan.FromMinutes(1));

        if (!endpointAllowed)
        {
            _logger.LogWarning("Rate limit exceeded for endpoint: {Endpoint} from IP: {IpAddress}", endpoint, ipAddress);
            await ReturnRateLimitExceeded(context, "Too many requests to this endpoint");
            return;
        }

        // Rate limiting por usuario (si está autenticado)
        if (!string.IsNullOrEmpty(userId))
        {
            var userAllowed = await _rateLimitingService.TryAcquireForUserAsync(
                userId,
                maxRequests: 200,
                window: TimeSpan.FromMinutes(1));

            if (!userAllowed)
            {
                _logger.LogWarning("Rate limit exceeded for user: {UserId}", userId);
                await ReturnRateLimitExceeded(context, "Too many requests for this user");
                return;
            }
        }

        await _next(context);
    }

    /// <summary>
    /// Obtiene la dirección IP real del cliente, considerando proxies y headers de forwarding.
    /// </summary>
    /// <param name="context">Contexto de la solicitud HTTP</param>
    /// <returns>Dirección IP del cliente</returns>
    /// <remarks>
    /// Verifica los siguientes headers en orden de prioridad:
    /// 1. X-Forwarded-For (para proxies)
    /// 2. X-Real-IP (para algunos proxies)
    /// 3. RemoteIpAddress (IP directa)
    /// </remarks>
    private string GetClientIpAddress(HttpContext context)
    {
        // Obtener IP real considerando proxies
        var forwardedHeader = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedHeader))
        {
            return forwardedHeader.Split(',')[0].Trim();
        }

        var realIpHeader = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIpHeader))
        {
            return realIpHeader;
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }

    /// <summary>
    /// Extrae el ID del usuario desde el contexto de autenticación.
    /// </summary>
    /// <param name="context">Contexto de la solicitud HTTP</param>
    /// <returns>ID del usuario o null si no está autenticado</returns>
    private string? GetUserIdFromContext(HttpContext context)
    {
        return context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    }

    /// <summary>
    /// Determina el número máximo de solicitudes permitidas para un endpoint específico.
    /// </summary>
    /// <param name="endpoint">Ruta del endpoint</param>
    /// <returns>Número máximo de solicitudes permitidas</returns>
    /// <remarks>
    /// Los endpoints sensibles como login tienen límites más restrictivos
    /// para prevenir ataques de fuerza bruta.
    /// </remarks>
    private int GetMaxRequestsForEndpoint(string endpoint)
    {
        return endpoint switch
        {
            "/api/user/login" => 5, // Login más restrictivo
            "/api/user/refresh" => 10, // Refresh token
            "/api/user/logout" => 20, // Logout
            _ => 50 // Endpoints generales
        };
    }

    /// <summary>
    /// Retorna una respuesta HTTP 429 (Too Many Requests) con headers apropiados.
    /// </summary>
    /// <param name="context">Contexto de la solicitud HTTP</param>
    /// <param name="message">Mensaje de error</param>
    /// <returns>Task que representa la operación asíncrona</returns>
    /// <remarks>
    /// Incluye headers estándar de rate limiting:
    /// - X-RateLimit-Limit: Límite de solicitudes
    /// - X-RateLimit-Remaining: Solicitudes restantes (0)
    /// - X-RateLimit-Reset: Tiempo de reset en timestamp Unix
    /// - Retry-After: Tiempo de espera en segundos
    /// </remarks>
    private async Task ReturnRateLimitExceeded(HttpContext context, string message)
    {
        context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
        context.Response.ContentType = "application/json";
        
        var response = new
        {
            error = "rate_limit_exceeded",
            message = message,
            retry_after = 60 // segundos
        };

        // Agregar headers de rate limiting
        context.Response.Headers.Add("X-RateLimit-Limit", "100");
        context.Response.Headers.Add("X-RateLimit-Remaining", "0");
        context.Response.Headers.Add("X-RateLimit-Reset", DateTimeOffset.UtcNow.AddMinutes(1).ToUnixTimeSeconds().ToString());
        context.Response.Headers.Add("Retry-After", "60");

        await context.Response.WriteAsJsonAsync(response);
    }
} 