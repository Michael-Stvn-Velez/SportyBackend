using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.UseCases.AuthUseCases;

namespace BackendSport.API.Controllers.v1.Admin;

/// <summary>
/// Controller for owner/admin user management
/// </summary>
[ApiController]
[Route("api/v1/admin/users")]
[Produces("application/json")]
public class OwnerUsersController : ControllerBase
{
    private readonly CreateOwnerUserUseCase _createOwnerUserUseCase;

    public OwnerUsersController(CreateOwnerUserUseCase createOwnerUserUseCase)
    {
        _createOwnerUserUseCase = createOwnerUserUseCase;
    }

    /// <summary>
    /// Create a new owner/admin user
    /// </summary>
    /// <param name="createOwnerUserDto">Owner user data</param>
    /// <returns>Created owner user</returns>
    /// <response code="201">Owner user created successfully</response>
    /// <response code="400">Validation error</response>
    [HttpPost]
    [ProducesResponseType(typeof(OwnerUserResponseDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateOwnerUser([FromBody] CreateOwnerUserDto createOwnerUserDto)
    {
        try
        {
            // Force default role ID and only one role
            // The DTO doesn't have roleIds, but the UseCase should assign it
            var response = await _createOwnerUserUseCase.ExecuteAsync(createOwnerUserDto);
            return CreatedAtAction(nameof(CreateOwnerUser), new { id = response.Id }, response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Owner user login
    /// </summary>
    /// <param name="loginDto">Login credentials</param>
    /// <param name="loginOwnerUserUseCase">Owner login use case</param>
    /// <returns>Authentication response</returns>
    /// <response code="200">Login successful</response>
    /// <response code="400">Invalid credentials</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequestDto loginDto, 
        [FromServices] LoginOwnerUserUseCase loginOwnerUserUseCase)
    {
        try
        {
            var result = await loginOwnerUserUseCase.ExecuteAsync(loginDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
