using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.UseCases.AuthUseCases;

namespace BackendSport.API.Controllers.v1.Users;

/// <summary>
/// Controller for managing user roles
/// </summary>
[ApiController]
[Route("api/v1/users/{userId}/roles")]
[Produces("application/json")]
public class UserRolesController : ControllerBase
{
    /// <summary>
    /// Assign role to user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="assignRoleDto">Role assignment data</param>
    /// <param name="asignarRolAUsuarioUseCase">Assign role use case</param>
    /// <returns>Assignment confirmation</returns>
    /// <response code="200">Role assigned successfully</response>
    /// <response code="400">Invalid request</response>
    [HttpPut]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AssignRole(
        string userId,
        [FromBody] AssignRoleRequestDto assignRoleDto,
        [FromServices] AsignarRolAUsuarioUseCase asignarRolAUsuarioUseCase)
    {
        try
        {
            if (assignRoleDto == null || string.IsNullOrEmpty(assignRoleDto.RoleId))
            {
                return BadRequest(new { message = "RoleId is required" });
            }

            var dto = new AsignarRolUsuarioDto
            {
                UserId = userId,
                RolId = assignRoleDto.RoleId
            };

            await asignarRolAUsuarioUseCase.ExecuteAsync(dto);
            return Ok(new { message = "Role assigned successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

/// <summary>
/// DTO for role assignment request
/// </summary>
public class AssignRoleRequestDto
{
    public string RoleId { get; set; } = string.Empty;
}
