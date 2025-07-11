using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.DTOs.LocationDTOs;
using BackendSport.Application.UseCases.LocationUseCases;

namespace BackendSport.API.Controllers.LocationControllers;

[ApiController]
[Route("api/[controller]")]
public class CityController : ControllerBase
{
    private readonly CreateCityUseCase _createCityUseCase;

    public CityController(CreateCityUseCase createCityUseCase)
    {
        _createCityUseCase = createCityUseCase;
    }

    [HttpPost]
    public async Task<ActionResult<CityDto>> CreateCity([FromBody] CreateCityDto createCityDto)
    {
        try
        {
            var city = await _createCityUseCase.ExecuteAsync(createCityDto);
            return CreatedAtAction(nameof(CreateCity), city);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
} 