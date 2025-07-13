using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.DTOs.PermisosDTOs;
using BackendSport.Application.UseCases.PermisosUseCases;

namespace BackendSport.API.Controllers.v1.Security;

/// <summary>
/// Controller for permissions management
/// </summary>
[ApiController]
[Route("api/v1/permissions")]
[Produces("application/json")]
public class PermissionsController : ControllerBase
{
    private readonly CreatePermisosUseCase _createPermisosUseCase;
    private readonly UpdatePermisosUseCase _updatePermisosUseCase;
    private readonly DeletePermisosUseCase _deletePermisosUseCase;
    private readonly GetPermisosByIdUseCase _getPermisosByIdUseCase;
    private readonly GetAllPermisosUseCase _getAllPermisosUseCase;

    public PermissionsController(
        CreatePermisosUseCase createPermisosUseCase,
        UpdatePermisosUseCase updatePermisosUseCase,
        DeletePermisosUseCase deletePermisosUseCase,
        GetPermisosByIdUseCase getPermisosByIdUseCase,
        GetAllPermisosUseCase getAllPermisosUseCase)
    {
        _createPermisosUseCase = createPermisosUseCase;
        _updatePermisosUseCase = updatePermisosUseCase;
        _deletePermisosUseCase = deletePermisosUseCase;
        _getPermisosByIdUseCase = getPermisosByIdUseCase;
        _getAllPermisosUseCase = getAllPermisosUseCase;
    }

    /// <summary>
    /// Get all permissions
    /// </summary>
    /// <returns>List of permissions</returns>
    /// <response code="200">Permissions retrieved successfully</response>
    /// <response code="400">Error retrieving permissions</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<PermisosDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAllPermissions()
    {
        try
        {
            var result = await _getAllPermisosUseCase.ExecuteAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get permission by ID
    /// </summary>
    /// <param name="id">Permission ID</param>
    /// <returns>Permission details</returns>
    /// <response code="200">Permission found</response>
    /// <response code="400">Error retrieving permission</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PermisosDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetPermissionById(string id)
    {
        try
        {
            var result = await _getPermisosByIdUseCase.ExecuteAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new permission
    /// </summary>
    /// <param name="permissionDto">Permission data (name and description)</param>
    /// <returns>Created permission</returns>
    /// <response code="201">Permission created successfully</response>
    /// <response code="400">Permission with this name already exists</response>
    [HttpPost]
    [ProducesResponseType(typeof(PermisosListDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreatePermission([FromBody] PermisosDto permissionDto)
    {
        try
        {
            var result = await _createPermisosUseCase.ExecuteAsync(permissionDto);
            return CreatedAtAction(nameof(GetPermissionById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing permission
    /// </summary>
    /// <param name="id">Permission ID</param>
    /// <param name="permissionUpdateDto">Updated permission data</param>
    /// <returns>Update confirmation</returns>
    /// <response code="200">Permission updated successfully</response>
    /// <response code="400">Validation error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdatePermission(string id, [FromBody] PermisosUpdateDto permissionUpdateDto)
    {
        try
        {
            var result = await _updatePermisosUseCase.ExecuteAsync(id, permissionUpdateDto);
            return Ok(new { message = "Permission updated successfully", updated = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a permission
    /// </summary>
    /// <param name="id">Permission ID</param>
    /// <returns>Deletion confirmation</returns>
    /// <response code="200">Permission deleted successfully</response>
    /// <response code="400">Validation error</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> DeletePermission(string id)
    {
        try
        {
            var result = await _deletePermisosUseCase.ExecuteAsync(id);
            return Ok(new { message = "Permission deleted successfully", deleted = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
