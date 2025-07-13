using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.DTOs.DeporteDTOs;
using BackendSport.Application.UseCases.AuthUseCases;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Domain.Services;

namespace BackendSport.API.Controllers.v1.Users;

/// <summary>
/// Controller for managing user sports
/// </summary>
[ApiController]
[Route("api/v1/users/{userId}/sports")]
[Produces("application/json")]
public class UserSportsController : ControllerBase
{
    /// <summary>
    /// Get user sports
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="userRepository">User repository</param>
    /// <returns>User sports list</returns>
    /// <response code="200">Sports retrieved successfully</response>
    /// <response code="404">User not found</response>
    [HttpGet]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetUserSports(
        string userId, 
        [FromServices] IUserRepository userRepository)
    {
        try
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { 
                userId = user.Id,
                sports = user.Sports,
                sportsCount = UserSportService.GetUserSportsCount(user)
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Assign sport to user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="assignSportDto">Sport assignment data</param>
    /// <param name="asignarDeporteAUsuarioUseCase">Assign sport use case</param>
    /// <returns>Assignment confirmation</returns>
    /// <response code="200">Sport assigned successfully</response>
    /// <response code="400">Invalid request</response>
    [HttpPost]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AssignSport(
        string userId,
        [FromBody] AssignSportRequestDto assignSportDto,
        [FromServices] AsignarDeporteAUsuarioUseCase asignarDeporteAUsuarioUseCase)
    {
        try
        {
            if (assignSportDto == null || string.IsNullOrEmpty(assignSportDto.SportId))
            {
                return BadRequest(new { message = "SportId is required" });
            }

            var dto = new AsignarDeporteUsuarioDto
            {
                UserId = userId,
                SportId = assignSportDto.SportId
            };

            await asignarDeporteAUsuarioUseCase.ExecuteAsync(dto);
            return Ok(new { message = "Sport assigned successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Remove sport from user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="sportId">Sport ID</param>
    /// <param name="removerDeporteDeUsuarioUseCase">Remove sport use case</param>
    /// <returns>Removal confirmation</returns>
    /// <response code="200">Sport removed successfully</response>
    /// <response code="400">Invalid request</response>
    [HttpDelete("{sportId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> RemoveSport(
        string userId,
        string sportId,
        [FromServices] RemoverDeporteDeUsuarioUseCase removerDeporteDeUsuarioUseCase)
    {
        try
        {
            var dto = new RemoverDeporteUsuarioDto
            {
                UserId = userId,
                SportId = sportId
            };

            await removerDeporteDeUsuarioUseCase.ExecuteAsync(dto);
            return Ok(new { message = "Sport removed successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Configure user sport
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="sportId">Sport ID</param>
    /// <param name="configureDto">Configuration data</param>
    /// <param name="configureUserSportUseCase">Configure sport use case</param>
    /// <returns>Configuration confirmation</returns>
    /// <response code="200">Sport configured successfully</response>
    /// <response code="400">Invalid request</response>
    [HttpPut("{sportId}/configuration")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> ConfigureUserSport(
        string userId,
        string sportId,
        [FromBody] ConfigureUserSportDto configureDto,
        [FromServices] ConfigureUserSportUseCase configureUserSportUseCase)
    {
        try
        {
            if (configureDto == null)
            {
                return BadRequest(new { message = "Configuration data is required" });
            }

            await configureUserSportUseCase.ExecuteAsync(userId, sportId, configureDto);
            return Ok(new { message = "Sport configured successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

/// <summary>
/// DTO for sport assignment request
/// </summary>
public class AssignSportRequestDto
{
    public string SportId { get; set; } = string.Empty;
}
