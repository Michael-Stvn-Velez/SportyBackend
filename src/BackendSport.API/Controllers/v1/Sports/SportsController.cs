using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.DTOs.DeporteDTOs;
using BackendSport.Application.UseCases.DeporteUseCases;
using BackendSport.API.Attributes;

namespace BackendSport.API.Controllers.v1.Sports;

/// <summary>
/// Controller for sports management
/// </summary>
[ApiController]
[Route("api/v1/sports")]
[Produces("application/json")]
[RequirePermission("administrar_deportes")]
public class SportsController : ControllerBase
{
    private readonly CreateDeporteUseCase _createUseCase;
    private readonly GetAllDeportesUseCase _getAllUseCase;
    private readonly GetDeporteByIdUseCase _getByIdUseCase;
    private readonly UpdateDeporteUseCase _updateUseCase;
    private readonly DeleteDeporteUseCase _deleteUseCase;

    public SportsController(
        CreateDeporteUseCase createUseCase,
        GetAllDeportesUseCase getAllUseCase,
        GetDeporteByIdUseCase getByIdUseCase,
        UpdateDeporteUseCase updateUseCase,
        DeleteDeporteUseCase deleteUseCase)
    {
        _createUseCase = createUseCase;
        _getAllUseCase = getAllUseCase;
        _getByIdUseCase = getByIdUseCase;
        _updateUseCase = updateUseCase;
        _deleteUseCase = deleteUseCase;
    }

    /// <summary>
    /// Get all sports
    /// </summary>
    /// <returns>List of sports</returns>
    /// <response code="200">Sports retrieved successfully</response>
    /// <response code="400">Error retrieving sports</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<DeporteListDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetAllSports()
    {
        try
        {
            var result = await _getAllUseCase.ExecuteAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get sport by ID
    /// </summary>
    /// <param name="id">Sport ID</param>
    /// <returns>Sport details</returns>
    /// <response code="200">Sport found</response>
    /// <response code="400">Error retrieving sport</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(DeporteListDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetSportById(string id)
    {
        try
        {
            var result = await _getByIdUseCase.ExecuteAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new sport
    /// </summary>
    /// <param name="dto">Sport data</param>
    /// <returns>Created sport</returns>
    /// <response code="201">Sport created successfully</response>
    /// <response code="400">Validation error or duplicate sport</response>
    [HttpPost]
    [ProducesResponseType(typeof(DeporteListDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateSport([FromBody] DeporteDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Sport name is required" });

            var result = await _createUseCase.ExecuteAsync(dto);
            return CreatedAtAction(nameof(GetSportById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing sport
    /// </summary>
    /// <param name="id">Sport ID</param>
    /// <param name="dto">Updated sport data</param>
    /// <returns>Updated sport</returns>
    /// <response code="200">Sport updated successfully</response>
    /// <response code="400">Validation error</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(DeporteListDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateSport(string id, [FromBody] DeporteUpdateDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { message = "Sport name is required" });

            var result = await _updateUseCase.ExecuteAsync(id, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a sport
    /// </summary>
    /// <param name="id">Sport ID</param>
    /// <returns>Deletion confirmation</returns>
    /// <response code="200">Sport deleted successfully</response>
    /// <response code="400">Error deleting sport</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> DeleteSport(string id)
    {
        try
        {
            var result = await _deleteUseCase.ExecuteAsync(id);
            return Ok(new { message = "Sport deleted successfully", deleted = result });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
