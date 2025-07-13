using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.UseCases.PermisosRolesUseCases;
using BackendSport.Application.DTOs.PermisosRolesDTOs;
using BackendSport.API.Attributes;

namespace BackendSport.API.Controllers.v1.Security;

/// <summary>
/// Controller for managing role permissions
/// </summary>
[ApiController]
[Route("api/v1/roles/{roleId}/permissions")]
[Produces("application/json")]
[RequirePermission("administrar_roles")]
public class RolePermissionsController : ControllerBase
{
    private readonly AsignarPermisosARolUseCase _asignarPermisosARolUseCase;
    private readonly RemoverPermisosARolUseCase _removerPermisosARolUseCase;
    private readonly ObtenerPermisosRolUseCase _obtenerPermisosRolUseCase;

    public RolePermissionsController(
        AsignarPermisosARolUseCase asignarPermisosARolUseCase,
        RemoverPermisosARolUseCase removerPermisosARolUseCase,
        ObtenerPermisosRolUseCase obtenerPermisosRolUseCase)
    {
        _asignarPermisosARolUseCase = asignarPermisosARolUseCase;
        _removerPermisosARolUseCase = removerPermisosARolUseCase;
        _obtenerPermisosRolUseCase = obtenerPermisosRolUseCase;
    }

    /// <summary>
    /// Get permissions for a role
    /// </summary>
    /// <param name="roleId">Role ID</param>
    /// <returns>List of role permissions</returns>
    /// <response code="200">Permissions retrieved successfully</response>
    /// <response code="400">Validation error</response>
    /// <response code="404">Role not found</response>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetRolePermissions(string roleId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(roleId))
            {
                return BadRequest(new { message = "Role ID is required" });
            }

            var permissions = await _obtenerPermisosRolUseCase.ExecuteAsync(roleId);
            return Ok(permissions);
        }
        catch (Exception ex)
        {
            // Check if it's a not found exception
            if (ex.Message.Contains("No se encontró") || ex.Message.Contains("not found"))
            {
                return NotFound(new { message = "Role not found" });
            }
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Assign permissions to role
    /// </summary>
    /// <param name="roleId">Role ID</param>
    /// <param name="dto">Permissions assignment data</param>
    /// <returns>Assignment confirmation</returns>
    /// <response code="200">Permissions assigned successfully</response>
    /// <response code="400">Validation error</response>
    [HttpPut]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AssignPermissions(string roleId, [FromBody] AsignarPermisosRolDto dto)
    {
        try
        {
            if (dto == null || dto.PermisosIds == null || !dto.PermisosIds.Any())
            {
                return BadRequest(new { message = "At least one permission ID is required" });
            }

            await _asignarPermisosARolUseCase.ExecuteAsync(roleId, dto);
            return Ok(new { message = "Permissions assigned successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Remove specific permission from role
    /// </summary>
    /// <param name="roleId">Role ID</param>
    /// <param name="permissionId">Permission ID</param>
    /// <returns>Removal confirmation</returns>
    /// <response code="200">Permission removed successfully</response>
    /// <response code="404">Permission was not assigned to this role</response>
    /// <response code="400">Validation error</response>
    [HttpDelete("{permissionId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RemovePermission(string roleId, string permissionId)
    {
        try
        {
            var dto = new RemoverPermisosRolDto
            {
                PermisosIds = new List<string> { permissionId }
            };

            var result = await _removerPermisosARolUseCase.ExecuteAsync(roleId, dto);
            
            if (result)
                return Ok(new { message = "Permission removed successfully" });
            else
                return NotFound(new { message = "Permission was not assigned to this role" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Remove multiple permissions from role
    /// </summary>
    /// <param name="roleId">Role ID</param>
    /// <param name="dto">Permissions removal data</param>
    /// <returns>Removal confirmation</returns>
    /// <response code="200">Permissions removed successfully</response>
    /// <response code="404">No permissions were assigned to this role</response>
    /// <response code="400">Validation error</response>
    [HttpPost("remove")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RemovePermissions(string roleId, [FromBody] RemoverPermisosRolDto dto)
    {
        try
        {
            if (dto == null || dto.PermisosIds == null || !dto.PermisosIds.Any())
            {
                return BadRequest(new { message = "At least one permission ID is required" });
            }

            var result = await _removerPermisosARolUseCase.ExecuteAsync(roleId, dto);
            
            if (result)
                return Ok(new { message = "Permissions removed successfully" });
            else
                return NotFound(new { message = "No permissions were removed (may not have been assigned)" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
