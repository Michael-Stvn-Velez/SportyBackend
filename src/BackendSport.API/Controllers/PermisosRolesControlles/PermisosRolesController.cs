using BackendSport.Application.UseCases.PermisosRolesUseCases;
using BackendSport.Application.DTOs.PermisosRolesDTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BackendSport.API.Controllers.PermisosRolesControlles
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermisosRolesController : ControllerBase
    {
        private readonly AsignarPermisosARolUseCase _asignarPermisosARolUseCase;
        private readonly RemoverPermisosARolUseCase _removerPermisosARolUseCase;
        private readonly ObtenerPermisosRolUseCase _obtenerPermisosRolUseCase;

        public PermisosRolesController(AsignarPermisosARolUseCase asignarPermisosARolUseCase, RemoverPermisosARolUseCase removerPermisosARolUseCase, ObtenerPermisosRolUseCase obtenerPermisosRolUseCase)
        {
            _asignarPermisosARolUseCase = asignarPermisosARolUseCase;
            _removerPermisosARolUseCase = removerPermisosARolUseCase;
            _obtenerPermisosRolUseCase = obtenerPermisosRolUseCase;
        }

        [HttpPost("asignar-permisos/{rolId}")]
        public async Task<IActionResult> AsignarPermisos(string rolId, [FromBody] AsignarPermisosRolDto dto)
        {
            try
            {
                await _asignarPermisosARolUseCase.ExecuteAsync(rolId, dto);
                return Ok(new { message = "Permisos asignados correctamente." });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("remover-permisos/{rolId}")]
        public async Task<IActionResult> RemoverPermisos(string rolId, [FromBody] RemoverPermisosRolDto dto)
        {
            try
            {
                var result = await _removerPermisosARolUseCase.ExecuteAsync(rolId, dto);
                if (result)
                    return Ok(new { message = "Permisos removidos correctamente." });
                else
                    return BadRequest(new { error = "Ningún permiso fue removido (puede que no estuvieran asignados)." });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("permisos/{rolId}")]
        public async Task<IActionResult> ObtenerPermisosRol(string rolId)
        {
            try
            {
                var permisos = await _obtenerPermisosRolUseCase.ExecuteAsync(rolId);
                return Ok(permisos);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
} 