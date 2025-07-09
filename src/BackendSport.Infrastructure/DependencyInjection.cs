using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BackendSport.Infrastructure.Persistence;
using BackendSport.Infrastructure.Services;
using BackendSport.Application.Services;
using BackendSport.Application.Interfaces.DeporteInterfaces;
using BackendSport.Infrastructure.Persistence.DeportePersistence;

namespace BackendSport.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurar MongoDB
        var mongoDbSettings = new MongoDbSettings();
        configuration.GetSection("MongoDbSettings").Bind(mongoDbSettings);
        
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));
        services.AddSingleton(mongoDbSettings);
        services.AddSingleton<MongoDbContext>();

        // Registrar servicios
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<BackendSport.Application.Services.IPermissionCheckerService, BackendSport.Application.Services.PermissionCheckerService>();
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IDeporteRepository, DeporteRepository>();
        

        return services;
    }
}