using BackendSport.Infrastructure;
using BackendSport.Application.Interfaces.RolInterfaces;
using BackendSport.Infrastructure.Persistence.RolPersistence;
using BackendSport.Application.UseCases.RolUseCases;
using BackendSport.Application.Interfaces.PermisosInterfaces;
using BackendSport.Infrastructure.Persistence.PermisosPersistence;
using BackendSport.Application.UseCases.PermisosUseCases;
using BackendSport.Application.UseCases.PermisosRolesUseCases;
using BackendSport.Application.UseCases.AuthUseCases;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Infrastructure.Persistence.AuthPersistence;
using BackendSport.Application.Interfaces.DeporteInterfaces;
using BackendSport.Infrastructure.Persistence.DeportePersistence;
using BackendSport.Application.UseCases.DeporteUseCases;
using BackendSport.Application.UseCases.LocationUseCases;
using BackendSport.API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurar CORS mejorado
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

// Configurar servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "BackendSport API",
        Version = "v1",
        Description = "API para gestión de usuarios con Clean Architecture y MongoDB - Seguridad Avanzada"
    });
    
    // Incluir comentarios XML para documentación
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // Configuración mejorada para JWT en Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Agregar servicios de Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

// Configurar autenticación JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<BackendSport.Infrastructure.Services.JwtSettings>();
if (jwtSettings != null)
{
    builder.Services.AddAuthentication(options =>
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
}

//Repositorios
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IPermisosRepository, PermisosRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IDeporteRepository, DeporteRepository>();

//Casos de uso
builder.Services.AddScoped<CreateRolUseCase>();
builder.Services.AddScoped<UpdateRolUseCase>();
builder.Services.AddScoped<DeleteRolUseCase>();
builder.Services.AddScoped<GetRolByIdUseCase>();
builder.Services.AddScoped<GetAllRolesUseCase>();
builder.Services.AddScoped<CreatePermisosUseCase>();
builder.Services.AddScoped<UpdatePermisosUseCase>();
builder.Services.AddScoped<DeletePermisosUseCase>();
builder.Services.AddScoped<GetPermisosByIdUseCase>();
builder.Services.AddScoped<GetAllPermisosUseCase>();
builder.Services.AddScoped<AsignarPermisosARolUseCase>();
builder.Services.AddScoped<RemoverPermisosARolUseCase>();
builder.Services.AddScoped<ObtenerPermisosRolUseCase>();
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddScoped<LoginUserUseCase>();
builder.Services.AddScoped<RefreshTokenUseCase>();
builder.Services.AddScoped<LogoutUserUseCase>();
builder.Services.AddScoped<LogoutAllUserDevicesUseCase>();
builder.Services.AddScoped<AsignarRolAUsuarioUseCase>();
builder.Services.AddScoped<CreateDeporteUseCase>();
builder.Services.AddScoped<GetAllDeportesUseCase>();
builder.Services.AddScoped<GetDeporteByIdUseCase>();
builder.Services.AddScoped<UpdateDeporteUseCase>();
builder.Services.AddScoped<DeleteDeporteUseCase>();
builder.Services.AddScoped<LoginOwnerUserUseCase>();

// Casos de uso de ubicación
builder.Services.AddScoped<GetLocationHierarchyUseCase>();
builder.Services.AddScoped<GetDocumentTypesByCountryUseCase>();
builder.Services.AddScoped<CreateCountryUseCase>();
builder.Services.AddScoped<CreateDepartmentUseCase>();
builder.Services.AddScoped<CreateMunicipalityUseCase>();
builder.Services.AddScoped<CreateCityUseCase>();
builder.Services.AddScoped<CreateLocalityUseCase>();
builder.Services.AddScoped<CreateDocumentTypeUseCase>();

var app = builder.Build();

// Configurar pipeline HTTP con middlewares de seguridad

// Middleware de headers de seguridad (debe ir primero)
app.UseMiddleware<SecurityHeadersMiddleware>();

// Middleware de sanitización de inputs
app.UseMiddleware<InputSanitizationMiddleware>();

// Middleware de rate limiting
app.UseMiddleware<RateLimitingMiddleware>();

// Habilitar Swagger en todos los entornos para desarrollo
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackendSport API V1");
    c.RoutePrefix = string.Empty; // Swagger en la raíz
    
    // Configuración adicional para Swagger UI
    c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    c.DefaultModelsExpandDepth(-1);
    
    // Configuración para mejorar la experiencia de usuario
    c.DisplayRequestDuration();
    c.EnableDeepLinking();
    c.EnableFilter();
});

// Configurar CORS - debe ir antes de UseAuthorization
app.UseCors("AllowSwagger");

app.UseHttpsRedirection();

// Autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
