using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BackendSport.Infrastructure.Persistence;
using BackendSport.Infrastructure.Services;
using BackendSport.Application.Services;
using BackendSport.Application.Interfaces.DeporteInterfaces;
using BackendSport.Infrastructure.Persistence.DeportePersistence;
using BackendSport.Application.Interfaces.LocationInterfaces;
using BackendSport.Infrastructure.Persistence.LocationPersistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BackendSport.Infrastructure;

/// <summary>
/// Clase estática que proporciona métodos de extensión para configurar
/// la inyección de dependencias de la capa de infraestructura.
/// </summary>
/// <remarks>
/// Esta clase centraliza toda la configuración de servicios de infraestructura:
/// - Configuración de MongoDB
/// - Configuración de JWT y autenticación
/// - Registro de servicios de seguridad
/// - Registro de repositorios
/// - Configuración de rate limiting
/// 
/// Implementa el patrón de configuración de .NET Core para una gestión
/// centralizada de dependencias.
/// </remarks>
public static class DependencyInjection
{
    /// <summary>
    /// Agrega todos los servicios de infraestructura al contenedor de dependencias.
    /// </summary>
    /// <param name="services">Colección de servicios de la aplicación</param>
    /// <param name="configuration">Configuración de la aplicación</param>
    /// <returns>Colección de servicios configurada</returns>
    /// <remarks>
    /// Este método configura:
    /// 1. MongoDB: Conexión, configuración y contexto
    /// 2. JWT: Configuración de tokens y autenticación
    /// 3. Autenticación: JWT Bearer con validación completa
    /// 4. Servicios de seguridad: Password, Token, Rate Limiting
    /// 5. Repositorios: Todos los repositorios de la aplicación
    /// 
    /// La configuración se lee desde appsettings.json y se valida
    /// para asegurar que todos los parámetros requeridos estén presentes.
    /// </remarks>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurar MongoDB
        var mongoDbSettings = new MongoDbSettings();
        configuration.GetSection("MongoDbSettings").Bind(mongoDbSettings);
        
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        services.AddSingleton(mongoDbSettings);
        services.AddSingleton<MongoDbContext>();

        // Configurar JWT
        var jwtSettings = new JwtSettings();
        configuration.GetSection("JwtSettings").Bind(jwtSettings);
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddSingleton(jwtSettings);

        // Configurar autenticación JWT
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                ValidateIssuer = jwtSettings.ValidateIssuer,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = jwtSettings.ValidateAudience,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = jwtSettings.ValidateLifetime,
                ClockSkew = TimeSpan.FromMinutes(jwtSettings.ClockSkewMinutes),
                RequireExpirationTime = true,
                RequireSignedTokens = true
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Add("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                },
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    var result = System.Text.Json.JsonSerializer.Serialize(new { error = "Unauthorized", message = "Token inválido o expirado" });
                    context.Response.WriteAsync(result);
                    return Task.CompletedTask;
                }
            };
        });

        // Registrar servicios de seguridad
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<BackendSport.Application.Services.IPermissionCheckerService, BackendSport.Application.Services.PermissionCheckerService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<RateLimitingService>();

        // Registrar repositorios
        services.AddScoped<IDeporteRepository, DeporteRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IMunicipalityRepository, MunicipalityRepository>();
        services.AddScoped<ILocalityRepository, LocalityRepository>();
        services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();

        return services;
    }
}