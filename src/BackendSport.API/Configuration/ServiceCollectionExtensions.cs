using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Application.Interfaces.DeporteInterfaces;
using BackendSport.Application.Interfaces.PermisosInterfaces;
using BackendSport.Application.Interfaces.RolInterfaces;
using BackendSport.Application.Interfaces.LocationInterfaces;
using BackendSport.Application.UseCases.AuthUseCases;
using BackendSport.Application.UseCases.DeporteUseCases;
using BackendSport.Application.UseCases.LocationUseCases;
using BackendSport.Application.UseCases.PermisosRolesUseCases;
using BackendSport.Application.UseCases.PermisosUseCases;
using BackendSport.Application.UseCases.RolUseCases;
using BackendSport.Infrastructure.Persistence.AuthPersistence;
using BackendSport.Infrastructure.Persistence.DeportePersistence;
using BackendSport.Infrastructure.Persistence.PermisosPersistence;
using BackendSport.Infrastructure.Persistence.RolPersistence;
using BackendSport.Infrastructure.Persistence.LocationPersistence;

namespace BackendSport.API.Configuration;

/// <summary>
/// Extensiones para configurar servicios de la aplicación
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configura todos los repositorios de la aplicación
    /// </summary>
    /// <param name="services">Colección de servicios</param>
    /// <returns>IServiceCollection para encadenamiento</returns>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IRolRepository, RolRepository>();
        services.AddScoped<IPermisosRepository, PermisosRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IDeporteRepository, DeporteRepository>();
        
        // Owner User Repository - Para autenticación de propietarios
        services.AddScoped<IOwnerUserRepository, OwnerUserRepository>();
        
        // Location Repositories - Para gestión de ubicaciones
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IMunicipalityRepository, MunicipalityRepository>();
        services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
        
        return services;
    }

    /// <summary>
    /// Configura todos los casos de uso de la aplicación
    /// </summary>
    /// <param name="services">Colección de servicios</param>
    /// <returns>IServiceCollection para encadenamiento</returns>
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        // Role Use Cases
        services.AddScoped<CreateRolUseCase>();
        services.AddScoped<UpdateRolUseCase>();
        services.AddScoped<DeleteRolUseCase>();
        services.AddScoped<GetRolByIdUseCase>();
        services.AddScoped<GetAllRolesUseCase>();

        // Permission Use Cases
        services.AddScoped<CreatePermisosUseCase>();
        services.AddScoped<UpdatePermisosUseCase>();
        services.AddScoped<DeletePermisosUseCase>();
        services.AddScoped<GetPermisosByIdUseCase>();
        services.AddScoped<GetAllPermisosUseCase>();

        // Role-Permission Use Cases
        services.AddScoped<AsignarPermisosARolUseCase>();
        services.AddScoped<RemoverPermisosARolUseCase>();
        services.AddScoped<ObtenerPermisosRolUseCase>();

        // Authentication Use Cases
        services.AddScoped<CreateUserUseCase>();
        services.AddScoped<LoginUserUseCase>();
        services.AddScoped<RefreshTokenUseCase>();
        services.AddScoped<LogoutUserUseCase>();
        services.AddScoped<LogoutAllUserDevicesUseCase>();
        services.AddScoped<AsignarRolAUsuarioUseCase>();
        services.AddScoped<LoginOwnerUserUseCase>();

        // Sport Use Cases
        services.AddScoped<CreateDeporteUseCase>();
        services.AddScoped<GetAllDeportesUseCase>();
        services.AddScoped<GetDeporteByIdUseCase>();
        services.AddScoped<UpdateDeporteUseCase>();
        services.AddScoped<DeleteDeporteUseCase>();

        // Location Use Cases
        services.AddScoped<GetLocationHierarchyUseCase>();
        services.AddScoped<GetDocumentTypesByCountryUseCase>();
        services.AddScoped<CreateCountryUseCase>();
        services.AddScoped<CreateDepartmentUseCase>();
        services.AddScoped<CreateMunicipalityUseCase>();
        services.AddScoped<CreateCityUseCase>();
        services.AddScoped<CreateLocalityUseCase>();
        services.AddScoped<CreateDocumentTypeUseCase>();

        return services;
    }
}
