using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;

namespace BackendSport.API.Configuration;

/// <summary>
/// Extensiones para configurar el mapeo de controladores con versionado automático
/// </summary>
public static class ControllerMappingExtensions
{
    /// <summary>
    /// Configura el mapeo de controladores con detección automática de versión por carpeta
    /// </summary>
    /// <param name="services">Colección de servicios</param>
    /// <returns>IServiceCollection para encadenamiento</returns>
    public static IServiceCollection AddVersionedControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            // Agregar convención para versionado automático basado en carpetas
            options.Conventions.Add(new VersionByNamespaceConvention());
        });

        return services;
    }

    /// <summary>
    /// Mapea controladores con configuración mejorada
    /// </summary>
    /// <param name="app">WebApplication instance</param>
    /// <returns>WebApplication para encadenamiento</returns>
    public static WebApplication MapVersionedControllers(this WebApplication app)
    {
        app.MapControllers();
        
        // Endpoint para información de versiones disponibles
        app.MapGet("/api/versions", (IServiceProvider services) =>
        {
            // Detectar automáticamente las versiones disponibles
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var availableVersions = new List<string>();
            
            foreach (var assembly in assemblies)
            {
                var controllerTypes = assembly.GetTypes()
                    .Where(t => t.Name.EndsWith("Controller") && !t.IsAbstract);
                
                foreach (var controllerType in controllerTypes)
                {
                    var namespaceName = controllerType.Namespace ?? string.Empty;
                    var versionMatch = System.Text.RegularExpressions.Regex.Match(namespaceName, @"\.v(\d+)\.");
                    
                    if (versionMatch.Success)
                    {
                        var version = $"v{versionMatch.Groups[1].Value}";
                        if (!availableVersions.Contains(version))
                        {
                            availableVersions.Add(version);
                        }
                    }
                    else if (namespaceName.Contains("Controllers") && !availableVersions.Contains("v0"))
                    {
                        availableVersions.Add("v0");
                    }
                }
            }

            availableVersions.Sort();

            return new
            {
                available_versions = availableVersions.ToArray(),
                current_version = "v1",
                default_version = "v0",
                deprecated_versions = new[] { "v0" },
                version_info = new
                {
                    total_versions = availableVersions.Count,
                    detection_method = "automatic_namespace_scanning",
                    last_updated = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC")
                },
                support_info = new
                {
                    v0 = new
                    {
                        status = "deprecated",
                        description = "Legacy controllers - Use v1 instead",
                        released = "2024-01-01",
                        deprecation_date = "2025-12-31",
                        migration_guide = "/api/migration/v0-to-v1"
                    },
                    v1 = new
                    {
                        status = "stable",
                        description = "Current stable API version with versioned structure",
                        released = "2025-01-01",
                        deprecation_date = (string?)null,
                        documentation = "/swagger/v1/swagger.json"
                    }
                },
                keyboard_shortcuts = new
                {
                    switch_to_v0 = "Ctrl+0",
                    switch_to_v1 = "Ctrl+1",
                    description = "Use keyboard shortcuts in Swagger UI for quick version switching"
                }
            };
        })
        .WithTags("API Information")
        .WithSummary("Get available API versions and their status");

        return app;
    }
}

/// <summary>
/// Convención para aplicar versionado automático basado en el namespace/carpeta
/// </summary>
public class VersionByNamespaceConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        var controllerNamespace = controller.ControllerType.Namespace ?? string.Empty;
        
        // Extraer versión del namespace (ej: .Controllers.v1.Auth -> v1)
        var versionMatch = System.Text.RegularExpressions.Regex.Match(
            controllerNamespace, 
            @"\.v(\d+)\."
        );
        
        if (versionMatch.Success)
        {
            var version = versionMatch.Groups[1].Value;
            
            // Aplicar versión al controlador mediante GroupName para Swagger
            controller.ApiExplorer.GroupName = $"v{version}";
        }
        else
        {
            // Si no está en una carpeta versionada, asignar v0 (legacy)
            controller.ApiExplorer.GroupName = "v0";
        }
    }
}
