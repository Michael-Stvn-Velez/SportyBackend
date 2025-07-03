using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.UseCases.AuthUseCases;

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
    /// Registra un nuevo usuario en el sistema
    /// </summary>
    /// <param name="createUserDto">Datos del usuario a registrar</param>
    /// <returns>Usuario creado exitosamente</returns>
    /// <response code="201">Usuario creado exitosamente</response>
    /// <response code="400">Datos de entrada inválidos</response>
    /// <response code="409">Ya existe un usuario con ese email</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost]
    [ProducesResponseType(typeof(UserResponseDto), 201)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(typeof(ProblemDetails), 409)]
    [ProducesResponseType(typeof(ProblemDetails), 500)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            var user = await _createUserUseCase.ExecuteAsync(createUserDto);
            
            return CreatedAtAction(nameof(CreateUser), new { id = user.Id }, user);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new ProblemDetails
            {
                Title = "Usuario ya existe",
                Detail = ex.Message,
                Status = 409
            });
        }
        catch
        {
            return StatusCode(500, new ProblemDetails
            {
                Title = "Error interno del servidor",
                Detail = "Ocurrió un error inesperado al crear el usuario",
                Status = 500
            });
        }
    }
} 