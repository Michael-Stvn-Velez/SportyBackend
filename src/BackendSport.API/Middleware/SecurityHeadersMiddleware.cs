namespace BackendSport.API.Middleware;

/// <summary>
/// Middleware para agregar headers de seguridad a todas las respuestas HTTP.
/// Implementa mejores prácticas de seguridad web para proteger contra ataques comunes.
/// </summary>
/// <remarks>
/// Este middleware agrega los siguientes headers de seguridad:
/// - X-Content-Type-Options: Previene MIME type sniffing
/// - X-Frame-Options: Previene clickjacking
/// - X-XSS-Protection: Habilita protección XSS del navegador
/// - Referrer-Policy: Controla información del referrer
/// - Permissions-Policy: Controla permisos del navegador
/// - Content-Security-Policy: Política de seguridad de contenido
/// - Strict-Transport-Security: Fuerza HTTPS (solo en HTTPS)
/// 
/// También elimina headers que pueden exponer información del servidor.
/// </remarks>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SecurityHeadersMiddleware> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del middleware de headers de seguridad.
    /// </summary>
    /// <param name="next">Siguiente middleware en el pipeline</param>
    /// <param name="logger">Logger para registrar eventos</param>
    public SecurityHeadersMiddleware(RequestDelegate next, ILogger<SecurityHeadersMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Procesa la solicitud HTTP agregando headers de seguridad a la respuesta.
    /// </summary>
    /// <param name="context">Contexto de la solicitud HTTP</param>
    /// <returns>Task que representa la operación asíncrona</returns>
    /// <remarks>
    /// Los headers se agregan antes de que la respuesta sea enviada al cliente.
    /// Algunos headers como HSTS solo se agregan cuando la conexión es HTTPS.
    /// </remarks>
    public async Task InvokeAsync(HttpContext context)
    {
        // Agregar headers de seguridad
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("X-Frame-Options", "DENY");
        context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
        context.Response.Headers.Add("Permissions-Policy", "geolocation=(), microphone=(), camera=()");
        
        // Content Security Policy
        context.Response.Headers.Add("Content-Security-Policy", 
            "default-src 'self'; " +
            "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
            "style-src 'self' 'unsafe-inline'; " +
            "img-src 'self' data: https:; " +
            "font-src 'self'; " +
            "connect-src 'self'; " +
            "frame-ancestors 'none';");

        // Strict Transport Security (solo en HTTPS)
        if (context.Request.IsHttps)
        {
            context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");
        }

        // Remover headers que pueden exponer información
        context.Response.Headers.Remove("Server");
        context.Response.Headers.Remove("X-Powered-By");

        await _next(context);
    }
} 