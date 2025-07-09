using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using BackendSport.Application.Services;
using System.Threading.Tasks;

namespace BackendSport.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequirePermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _permission;

        public RequirePermissionAttribute(string permission)
        {
            _permission = permission;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            var authHeader = httpContext.Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader))
            {
                context.Result = new UnauthorizedObjectResult(new { 
                    mensaje = "Token de autorización requerido. Asegúrate de incluir 'Bearer {token}' en el header Authorization."
                });
                return;
            }

            if (!authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new UnauthorizedObjectResult(new { 
                    mensaje = "Formato de token incorrecto. Debe ser 'Bearer {token}'"
                });
                return;
            }

            var accessToken = authHeader.Substring("Bearer ".Length).Trim();
            if (string.IsNullOrEmpty(accessToken))
            {
                context.Result = new UnauthorizedObjectResult(new { 
                    mensaje = "Token vacío. Proporciona un token válido."
                });
                return;
            }

            var authorizationService = httpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var hasPermission = await authorizationService.ValidatePermissionAsync(accessToken, _permission);
            if (!hasPermission)
            {
                context.Result = new ObjectResult(new { 
                    mensaje = $"No tienes permisos para: {_permission}. Verifica que tu token contenga este permiso."
                }) { StatusCode = 403 };
                return;
            }
        }
    }
} 