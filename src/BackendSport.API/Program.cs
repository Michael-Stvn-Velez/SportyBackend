using BackendSport.Infrastructure;
using BackendSport.Application.Interfaces.RolInterfaces;
using BackendSport.Infrastructure.Persistence.RolPersistence;
using BackendSport.Application.UseCases.RolUseCases;
using BackendSport.Application.Interfaces.PermisosInterfaces;
using BackendSport.Infrastructure.Persistence.PermisosPersistence;
using BackendSport.Application.UseCases.PermisosUseCases;
using BackendSport.Application.UseCases.PermisosRolesUseCases;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "BackendSport API",
        Version = "v1",
        Description = "API para gestión de usuarios con Clean Architecture y MongoDB"
    });
    
    // Incluir comentarios XML para documentación
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Agregar servicios de Infrastructure
builder.Services.AddInfrastructure(builder.Configuration);

//Repositorios
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IPermisosRepository, PermisosRepository>();

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

var app = builder.Build();

// Configurar pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BackendSport API V1");
        c.RoutePrefix = string.Empty; // Swagger en la raíz
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
