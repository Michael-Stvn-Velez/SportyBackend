using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BackendSport.Application.Services;
using System.Security.Claims;

namespace BackendSport.API.Attributes;

/// <summary>
/// Atributo de autorización personalizado para JWT que permite control granular
/// sobre roles y permisos requeridos para acceder a endpoints específicos.
/// </summary>
/// <remarks>
/// Este atributo implementa IAuthorizationFilter para interceptar solicitudes
/// antes de que lleguen a los controladores. Proporciona:
/// - Verificación de autenticación JWT
/// - Validación de roles requeridos
/// - Verificación de permisos específicos
/// - Respuestas HTTP apropiadas para casos de autorización fallida
/// 
/// Se puede aplicar a nivel de clase o método para control granular.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class JwtAuthorizationAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _requiredRoles;
    private readonly string[] _requiredPermissions;

    /// <summary>
    /// Inicializa una nueva instancia del atributo de autorización JWT.
    /// </summary>
    /// <param name="requiredRoles">Roles requeridos para acceder al endpoint</param>
    /// <param name="requiredPermissions">Permisos específicos requeridos</param>
    /// <remarks>
    /// Si no se especifican roles o permisos, solo se verifica la autenticación.
    /// Los roles se verifican contra los claims de tipo ClaimTypes.Role.
    /// Los permisos se verifican usando el servicio IPermissionCheckerService.
    /// </remarks>
    public JwtAuthorizationAttribute(string[]? requiredRoles = null, string[]? requiredPermissions = null)
    {
        _requiredRoles = requiredRoles ?? Array.Empty<string>();
        _requiredPermissions = requiredPermissions ?? Array.Empty<string>();
    }

    /// <summary>
    /// Ejecuta la lógica de autorización para la solicitud actual.
    /// </summary>
    /// <param name="context">Contexto del filtro de autorización</param>
    /// <remarks>
    /// El método verifica en el siguiente orden:
    /// 1. Autenticación del usuario (presencia de token JWT válido)
    /// 2. Roles requeridos (si se especifican)
    /// 3. Permisos específicos (si se especifican)
    /// 
    /// Si alguna verificación falla, se establece el resultado apropiado:
    /// - 401 Unauthorized para falta de autenticación
    /// - 403 Forbidden para falta de autorización
    /// </remarks>
    public async void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedObjectResult(new
            {
                error = "unauthorized",
                message = "Token de autenticación requerido"
            });
            return;
        }

        // Verificar roles si se especifican
        if (_requiredRoles.Length > 0)
        {
            var userRoles = user.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToArray();

            if (!_requiredRoles.Any(role => userRoles.Contains(role)))
            {
                context.Result = new ForbidResult();
                return;
            }
        }

        // Verificar permisos si se especifican
        if (_requiredPermissions.Length > 0)
        {
            // Aquí podrías implementar la lógica de verificación de permisos
            // usando el servicio IPermissionCheckerService
            var permissionService = context.HttpContext.RequestServices
                .GetService<BackendSport.Application.Services.IPermissionCheckerService>();

            if (permissionService != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    foreach (var permission in _requiredPermissions)
                    {
                        if (!await permissionService.UserHasPermissionAsync(userId, permission))
                        {
                            context.Result = new ForbidResult();
                            return;
                        }
                    }
                }
            }
        }
    }
} 