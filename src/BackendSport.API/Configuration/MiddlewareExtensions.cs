using BackendSport.API.Middleware;

namespace BackendSport.API.Configuration;

/// <summary>
/// Extensiones para configurar el pipeline de middleware
/// </summary>
public static class MiddlewareExtensions
{
    /// <summary>
    /// Configura todos los middlewares de seguridad en el orden correcto
    /// </summary>
    /// <param name="app">WebApplication instance</param>
    /// <returns>WebApplication para encadenamiento</returns>
    public static WebApplication UseSecurityMiddleware(this WebApplication app)
    {
        // Los middlewares de seguridad deben ejecutarse en este orden específico

        // 1. Headers de seguridad (debe ir primero)
        app.UseMiddleware<SecurityHeadersMiddleware>();

        // 2. Sanitización de inputs
        app.UseMiddleware<InputSanitizationMiddleware>();

        // 3. Rate limiting
        app.UseMiddleware<RateLimitingMiddleware>();

        return app;
    }

    /// <summary>
    /// Configura Swagger UI con configuración mejorada - SOLO EN DESARROLLO
    /// </summary>
    /// <param name="app">WebApplication instance</param>
    /// <returns>WebApplication para encadenamiento</returns>
    public static WebApplication UseSwaggerWithUI(this WebApplication app)
    {
        // Solo habilitar Swagger en entorno de desarrollo
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                // Configurar múltiples versiones con selector
                c.SwaggerEndpoint("/swagger/v0/swagger.json", "BackendSport API v0 (Legacy)");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackendSport API v1 (Current)");
                c.RoutePrefix = string.Empty; // Swagger en la raíz

                // Configuración adicional para Swagger UI
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                c.DefaultModelsExpandDepth(-1);

                // Configuración para mejorar la experiencia de usuario
                c.DisplayRequestDuration();
                c.EnableDeepLinking();
                c.EnableFilter();
                c.ShowExtensions();
                c.EnableValidator();

                // Configuración de tema y UI
                c.DefaultModelExpandDepth(2);
                c.DefaultModelsExpandDepth(-1);
                c.DisplayOperationId();

                // Configuración para selector de versiones
                c.EnableTryItOutByDefault();
                
                // JavaScript personalizado para mejorar el selector de versiones
                c.InjectJavascript("/js/swagger-custom.js");
                c.InjectStylesheet("/css/swagger-custom.css");
            });
        }

        return app;
    }

    /// <summary>
    /// Configura el pipeline HTTP básico (sin mapeo de controladores)
    /// </summary>
    /// <param name="app">WebApplication instance</param>
    /// <returns>WebApplication para encadenamiento</returns>
    public static WebApplication ConfigureBasicHttpPipeline(this WebApplication app)
    {
        // CORS - debe ir antes de UseAuthorization
        app.UseCors("AllowSwagger");

        // HTTPS Redirection
        app.UseHttpsRedirection();

        // Authentication & Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}
