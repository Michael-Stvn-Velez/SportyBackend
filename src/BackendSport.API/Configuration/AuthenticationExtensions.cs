using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace BackendSport.API.Configuration;

/// <summary>
/// Métodos de extensión para configurar JWT y Swagger
/// </summary>
public static class AuthenticationExtensions
{
    /// <summary>
    /// Configura la autenticación JWT
    /// </summary>
    /// <param name="services">Colección de servicios</param>
    /// <param name="configuration">Configuración de la aplicación</param>
    /// <returns>IServiceCollection para encadenamiento</returns>
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings").Get<BackendSport.Infrastructure.Services.JwtSettings>();
        if (jwtSettings == null) return services;

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
                        context.Response.Headers["Token-Expired"] = "true";
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

        return services;
    }

    /// <summary>
    /// Configura Swagger con autenticación JWT y versionado - SOLO EN DESARROLLO
    /// </summary>
    /// <param name="services">Colección de servicios</param>
    /// <param name="environment">Entorno de ejecución</param>
    /// <returns>IServiceCollection para encadenamiento</returns>
    public static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services, IWebHostEnvironment environment)
    {
        // Solo registrar Swagger en entorno de desarrollo
        if (environment.IsDevelopment())
        {
            services.AddSwaggerGen(c =>
            {
                // Configuración para múltiples versiones
                c.SwaggerDoc("v0", new OpenApiInfo
                {
                    Title = "BackendSport API - Legacy",
                    Version = "v0",
                    Description = "API Legacy - Controladores sin versión específica [DEPRECATED]",
                    Contact = new OpenApiContact
                    {
                        Name = "SportyBackend Team",
                        Email = "support@sportybackend.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "BackendSport API",
                    Version = "v1",
                    Description = "API para gestión de usuarios con Clean Architecture y MongoDB - Seguridad Avanzada [DESARROLLO]",
                    Contact = new OpenApiContact
                    {
                        Name = "SportyBackend Team",
                        Email = "support@sportybackend.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                // Incluir comentarios XML para documentación
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                // Configuración JWT para Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        return services;
    }
}
