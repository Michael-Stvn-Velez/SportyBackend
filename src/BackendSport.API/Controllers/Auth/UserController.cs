using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.DTOs.DeporteDTOs;
using BackendSport.Application.UseCases.AuthUseCases;
using BackendSport.Application.Services;
using BackendSport.Infrastructure.Services;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Domain.Entities.AuthEntities;

namespace BackendSport.API.Controllers.Auth;

/// <summary>
/// Controlador para gestionar usuarios
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly CreateUserUseCase _createUserUseCase;

    public UserController(CreateUserUseCase createUserUseCase)
    {
        _createUserUseCase = createUserUseCase;
    }

    /// <summary>
    /// Crea un nuevo usuario.
    /// </summary>
    /// <param name="createUserDto">Datos del usuario a crear</param>
    /// <returns>El usuario creado</returns>
    /// <response code="201">Usuario creado correctamente</response>
    /// <response code="400">Ya existe un usuario con ese email</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserResponseDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            var user = await _createUserUseCase.ExecuteAsync(createUserDto);
            
            return CreatedAtAction(nameof(CreateUser), new { id = user.Id }, user);
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDto, [FromServices] LoginUserUseCase loginUserUseCase)
    {
        try
        {
            var result = await loginUserUseCase.ExecuteAsync(loginDto);
            return Ok(result);
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto, [FromServices] RefreshTokenUseCase refreshTokenUseCase)
    {
        try
        {
            if (dto == null || string.IsNullOrEmpty(dto.RefreshToken))
            {
                return BadRequest(new { mensaje = "Refresh token es requerido" });
            }

            var result = await refreshTokenUseCase.ExecuteAsync(dto);
            
            return Ok(result);
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] RefreshTokenRequestDto dto, [FromServices] LogoutUserUseCase logoutUserUseCase)
    {
        try
        {
            if (dto == null || string.IsNullOrEmpty(dto.RefreshToken))
            {
                return BadRequest(new { mensaje = "Refresh token es requerido" });
            }

            await logoutUserUseCase.ExecuteAsync(dto);
            return Ok(new { mensaje = "Sesión cerrada correctamente" });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPost("logout-all")]
    public async Task<IActionResult> LogoutAll([FromServices] LogoutAllUserDevicesUseCase logoutAllUserDevicesUseCase)
    {
        try
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return BadRequest(new { mensaje = "Access token es requerido en el header Authorization" });
            }
            var accessToken = authHeader.Substring("Bearer ".Length).Trim();
            await logoutAllUserDevicesUseCase.ExecuteAsync(accessToken);
            return Ok(new { mensaje = "Sesión cerrada en todos los dispositivos" });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] AsignarRolUsuarioDto dto, [FromServices] AsignarRolAUsuarioUseCase asignarRolAUsuarioUseCase)
    {
        try
        {
            if (dto == null || string.IsNullOrEmpty(dto.UserId) || string.IsNullOrEmpty(dto.RolId))
            {
                return BadRequest(new { mensaje = "UserId y RolId son requeridos" });
            }
            await asignarRolAUsuarioUseCase.ExecuteAsync(dto);
            return Ok(new { mensaje = "Rol asignado correctamente" });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPost("assign-sport")]
    public async Task<IActionResult> AssignSport([FromBody] AsignarDeporteUsuarioDto dto, [FromServices] AsignarDeporteAUsuarioUseCase asignarDeporteAUsuarioUseCase)
    {
        try
        {
            if (dto == null || string.IsNullOrEmpty(dto.UserId) || string.IsNullOrEmpty(dto.SportId))
            {
                return BadRequest(new { mensaje = "UserId y SportId son requeridos" });
            }
            await asignarDeporteAUsuarioUseCase.ExecuteAsync(dto);
            return Ok(new { mensaje = "Deporte asignado correctamente" });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("remove-sport")]
    public async Task<IActionResult> RemoveSport([FromBody] RemoverDeporteUsuarioDto dto, [FromServices] RemoverDeporteDeUsuarioUseCase removerDeporteDeUsuarioUseCase)
    {
        try
        {
            if (dto == null || string.IsNullOrEmpty(dto.UserId) || string.IsNullOrEmpty(dto.SportId))
            {
                return BadRequest(new { mensaje = "UserId y SportId son requeridos" });
            }
            await removerDeporteDeUsuarioUseCase.ExecuteAsync(dto);
            return Ok(new { mensaje = "Deporte removido correctamente" });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpGet("{userId}/sports")]
    public async Task<IActionResult> GetUserSports(string userId, [FromServices] IUserRepository userRepository)
    {
        try
        {
            var user = await userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado" });
            }

            return Ok(new { 
                userId = user.Id,
                sports = user.Sports,
                sportsCount = user.GetSportsCount()
            });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPost("{userId}/sports/{sportId}/configure")]
    public async Task<IActionResult> ConfigureUserSport(string userId, string sportId, [FromBody] ConfigureUserSportDto dto, [FromServices] ConfigureUserSportUseCase configureUserSportUseCase)
    {
        try
        {
            if (dto == null)
            {
                return BadRequest(new { mensaje = "Datos de configuración son requeridos" });
            }

            await configureUserSportUseCase.ExecuteAsync(userId, sportId, dto);
            return Ok(new { mensaje = "Deporte configurado correctamente" });
        }
        catch (System.Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}