// Core ASP.NET Core
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Application Layer
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Application.Interfaces.DeporteInterfaces;
using BackendSport.Application.Interfaces.PermisosInterfaces;
using BackendSport.Application.Interfaces.RolInterfaces;
using BackendSport.Application.UseCases.AuthUseCases;
using BackendSport.Application.UseCases.DeporteUseCases;
using BackendSport.Application.UseCases.LocationUseCases;
using BackendSport.Application.UseCases.PermisosRolesUseCases;
using BackendSport.Application.UseCases.PermisosUseCases;
using BackendSport.Application.UseCases.RolUseCases;

// Infrastructure Layer
using BackendSport.Infrastructure;
using BackendSport.Infrastructure.Persistence.AuthPersistence;
using BackendSport.Infrastructure.Persistence.DeportePersistence;
using BackendSport.Infrastructure.Persistence.PermisosPersistence;
using BackendSport.Infrastructure.Persistence.RolPersistence;

// API Layer
using BackendSport.API.Configuration;
using BackendSport.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// CORS Configuration - Configurar políticas de CORS para desarrollo
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSwagger", policy =>
    {
        policy.WithOrigins("http://localhost:5001", "https://localhost:5001")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("X-RateLimit-Limit", "X-RateLimit-Remaining", "X-RateLimit-Reset", "Retry-After")
              .SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});

// API Documentation & Controllers - Configurar Swagger y controladores versionados
builder.Services.AddVersionedControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwt(builder.Environment); // Solo en desarrollo

// Infrastructure Services - Registrar servicios de infraestructura
builder.Services.AddInfrastructure(builder.Configuration);

// Authentication - Configurar autenticación JWT
builder.Services.AddJwtAuthentication(builder.Configuration);

// GraphQL - Configurar servicios de GraphQL
builder.Services.AddGraphQLServices(builder.Environment);

// Dependency Injection - Registrar repositorios y casos de uso
builder.Services.AddRepositories()
                .AddUseCases();

// Build Application
var app = builder.Build();

// Static Files - Habilitar archivos estáticos para Swagger personalizado
app.UseStaticFiles();

// Security Middleware Pipeline - Aplicar middlewares de seguridad en orden
app.UseSecurityMiddleware();

// Development Tools - Habilitar Swagger solo en desarrollo
app.UseSwaggerWithUI();

// HTTP Pipeline Configuration - Configurar pipeline completo
app.UseCors("AllowSwagger");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Controller Mapping - Mapear controladores con versionado automático
app.MapVersionedControllers();

// GraphQL - Configurar endpoint de GraphQL
app.UseGraphQLServices(builder.Environment);

// Application Startup
app.Run();
