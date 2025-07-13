using BackendSport.API.GraphQL.Types;
using BackendSport.API.GraphQL.Queries;
using BackendSport.API.GraphQL.Mutations;
using BackendSport.API.GraphQL.Subscriptions;

namespace BackendSport.API.Configuration;

/// <summary>
/// Extensiones para configurar GraphQL
/// </summary>
public static class GraphQLExtensions
{
    /// <summary>
    /// Configura los servicios de GraphQL
    /// </summary>
    /// <param name="services">Colección de servicios</param>
    /// <param name="environment">Entorno de la aplicación</param>
    /// <returns>IServiceCollection para encadenamiento</returns>
    public static IServiceCollection AddGraphQLServices(this IServiceCollection services, IWebHostEnvironment environment)
    {
        var graphqlBuilder = services
            .AddGraphQLServer()
            // Configurar esquema principal
            .AddQueryType()
            .AddMutationType()
            .AddSubscriptionType()
            
            // Registrar tipos de entidades
            .AddType<UserType>()
            .AddType<DeporteType>()
            .AddType<RolType>()
            .AddType<PermisosType>()
            .AddType<CountryType>()
            .AddType<DepartmentType>()
            .AddType<MunicipalityType>()
            .AddType<DocumentTypeType>()
            
            // Registrar queries
            .AddTypeExtension<UserQueries>()
            .AddTypeExtension<DeporteQueries>()
            .AddTypeExtension<SecurityQueries>()
            .AddTypeExtension<LocationQueries>()
            
            // Registrar mutations
            .AddTypeExtension<UserMutations>()
            .AddTypeExtension<DeporteMutations>()
            .AddTypeExtension<SecurityMutations>()
            
            // Registrar subscriptions
            .AddTypeExtension<SystemSubscriptions>()
            
            // Configurar autorización
            .AddAuthorization()
            
            // Configurar suscripciones en memoria
            .AddInMemorySubscriptions()
            
            // Configurar filtros y ordenamiento
            .AddFiltering()
            .AddSorting()
            .AddProjections()
            
            // Configurar manejo de errores
            .AddErrorFilter<GraphQLErrorFilter>()
            
            // Configuraciones adicionales
            .ModifyRequestOptions(opt =>
            {
                opt.IncludeExceptionDetails = environment.IsDevelopment();
            });

        // Solo en desarrollo: habilitar introspección
        if (environment.IsDevelopment())
        {
            // Permitir introspección del esquema en desarrollo
            graphqlBuilder.AllowIntrospection(true);
        }

        return services;
    }

    /// <summary>
    /// Configura el middleware de GraphQL
    /// </summary>
    /// <param name="app">Application builder</param>
    /// <param name="environment">Entorno de la aplicación</param>
    /// <returns>IApplicationBuilder para encadenamiento</returns>
    public static IApplicationBuilder UseGraphQLServices(this IApplicationBuilder app, IWebHostEnvironment environment)
    {
        // WebSocket para suscripciones
        app.UseWebSockets();
        
        // Configurar routing y mapear GraphQL
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            // Endpoint principal de GraphQL para consultas de datos
            endpoints.MapGraphQL("/graphql");
        });

        return app;
    }
}

/// <summary>
/// Filtro personalizado para errores de GraphQL
/// </summary>
public class GraphQLErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        return error.Exception switch
        {
            UnauthorizedAccessException => ErrorBuilder.FromError(error)
                .SetMessage("No tienes permisos para realizar esta operación")
                .SetCode("UNAUTHORIZED")
                .Build(),
                
            ArgumentException => ErrorBuilder.FromError(error)
                .SetMessage("Los datos proporcionados no son válidos")
                .SetCode("INVALID_ARGUMENT")
                .Build(),
                
            InvalidOperationException => ErrorBuilder.FromError(error)
                .SetMessage(error.Exception.Message)
                .SetCode("INVALID_OPERATION")
                .Build(),
                
            _ => ErrorBuilder.FromError(error)
                .SetMessage("Ha ocurrido un error interno")
                .SetCode("INTERNAL_ERROR")
                .Build()
        };
    }
}
