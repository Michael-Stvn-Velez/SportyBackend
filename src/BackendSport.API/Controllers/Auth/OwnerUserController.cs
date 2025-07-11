using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Application.UseCases.AuthUseCases;

namespace BackendSport.API.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class OwnerUserController : ControllerBase
{
    private readonly CreateOwnerUserUseCase _createOwnerUserUseCase;

    public OwnerUserController(CreateOwnerUserUseCase createOwnerUserUseCase)
    {
        _createOwnerUserUseCase = createOwnerUserUseCase;
    }

    [HttpPost]
    public async Task<ActionResult<OwnerUserResponseDto>> CreateOwnerUser([FromBody] CreateOwnerUserDto createOwnerUserDto)
    {
        try
        {
            // Forzar el rolId por defecto y solo uno
            // El DTO no tiene rolIds, pero el UseCase debe asignarlo
            var response = await _createOwnerUserUseCase.ExecuteAsync(createOwnerUserDto);
            return CreatedAtAction(nameof(CreateOwnerUser), new { id = response.Id }, response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto loginDto, [FromServices] LoginOwnerUserUseCase loginOwnerUserUseCase)
    {
        try
        {
            var result = await loginOwnerUserUseCase.ExecuteAsync(loginDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
} 