using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BackendSport.Infrastructure.Persistence;
using BackendSport.Infrastructure.Services;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Application.Services;
using BackendSport.Application.UseCases.AuthUseCases;
using BackendSport.Infrastructure.Persistence.AuthPersistence;

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

        // Registrar repositorios
        services.AddScoped<IUserRepository, UserRepository>();

        // Registrar servicios
        services.AddScoped<IPasswordService, PasswordService>();

        // Registrar casos de uso
        services.AddScoped<CreateUserUseCase>();

        return services;
    }
}