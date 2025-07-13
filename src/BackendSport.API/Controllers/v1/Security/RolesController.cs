using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.DTOs.RolDTOs;
using BackendSport.Application.UseCases.RolUseCases;

namespace BackendSport.API.Controllers.v1.Security;

/// <summary>
/// Controller for roles management
/// </summary>
[ApiController]
[Route("api/v1/roles")]
[Produces("application/json")]
public class RolesController : ControllerBase
{
    private readonly CreateRolUseCase _createRolUseCase;
    private readonly UpdateRolUseCase _updateRolUseCase;
    private readonly DeleteRolUseCase _deleteRolUseCase;
    private readonly GetRolByIdUseCase _getRolByIdUseCase;
    private readonly GetAllRolesUseCase _getAllRolesUseCase;

    public RolesController(
        CreateRolUseCase createRolUseCase,
        UpdateRolUseCase updateRolUseCase,
        DeleteRolUseCase deleteRolUseCase,
        GetRolByIdUseCase getRolByIdUseCase,
        GetAllRolesUseCase getAllRolesUseCase)
    {
        _createRolUseCase = createRolUseCase;
        _updateRolUseCase = updateRolUseCase;
        _deleteRolUseCase = deleteRolUseCase;
        _getRolByIdUseCase = getRolByIdUseCase;
        _getAllRolesUseCase = getAllRolesUseCase;
    }

    /// <summary>
    /// Get all roles
    /// </summary>
    /// <returns>List of roles</returns>
    /// <response code="200">Roles retrieved successfully</response>
    /// <response code="400">Error retrieving roles</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<RolDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAllRoles()
    {
        try
        {
            var result = await _getAllRolesUseCase.ExecuteAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get role by ID
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <returns>Role details</returns>
    /// <response code="200">Role found</response>
    /// <response code="400">Error retrieving role</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(RolDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetRoleById(string id)
    {
        try
        {
            var result = await _getRolByIdUseCase.ExecuteAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new role
    /// </summary>
    /// <param name="rolDto">Role data (name and description)</param>
    /// <returns>Created role</returns>
    /// <response code="201">Role created successfully</response>
    /// <response code="400">Role with this name already exists</response>
    [HttpPost]
    [ProducesResponseType(typeof(RolListDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateRole([FromBody] RolDto rolDto)
    {
        try
        {
            var result = await _createRolUseCase.ExecuteAsync(rolDto);
            return CreatedAtAction(nameof(GetRoleById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing role
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <param name="rolUpdateDto">Updated role data</param>
    /// <returns>Update confirmation</returns>
    /// <response code="200">Role updated successfully</response>
    /// <response code="400">Validation error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateRole(string id, [FromBody] RolUpdateDto rolUpdateDto)
    {
        try
        {
            var result = await _updateRolUseCase.ExecuteAsync(id, rolUpdateDto);
            return Ok(new { message = "Role updated successfully", updated = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a role
    /// </summary>
    /// <param name="id">Role ID</param>
    /// <returns>Deletion confirmation</returns>
    /// <response code="200">Role deleted successfully</response>
    /// <response code="400">Validation error</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> DeleteRole(string id)
    {
        try
        {
            var result = await _deleteRolUseCase.ExecuteAsync(id);
            return Ok(new { message = "Role deleted successfully", deleted = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
