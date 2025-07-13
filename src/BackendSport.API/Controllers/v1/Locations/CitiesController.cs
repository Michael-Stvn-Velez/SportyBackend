using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.DTOs.LocationDTOs;
using BackendSport.Application.UseCases.LocationUseCases;
using BackendSport.API.Attributes;

namespace BackendSport.API.Controllers.v1.Locations;

/// <summary>
/// Controller for cities management
/// </summary>
[ApiController]
[Route("api/v1/states/{stateId}/cities")]
[Produces("application/json")]
public class CitiesController : ControllerBase
{
    private readonly CreateMunicipalityUseCase _createMunicipalityUseCase;
    private readonly CreateLocalityUseCase _createLocalityUseCase;
    private readonly CreateCityUseCase _createCityUseCase;

    public CitiesController(
        CreateMunicipalityUseCase createMunicipalityUseCase,
        CreateLocalityUseCase createLocalityUseCase,
        CreateCityUseCase createCityUseCase)
    {
        _createMunicipalityUseCase = createMunicipalityUseCase;
        _createLocalityUseCase = createLocalityUseCase;
        _createCityUseCase = createCityUseCase;
    }

    /// <summary>
    /// Create a new municipality/city
    /// </summary>
    /// <param name="stateId">State/Department ID</param>
    /// <param name="createMunicipalityDto">Municipality data</param>
    /// <returns>Created municipality</returns>
    /// <response code="201">Municipality created successfully</response>
    /// <response code="400">Validation error</response>
    [HttpPost]
    [RequirePermission("administrar_parametros")]
    [ProducesResponseType(typeof(MunicipalityDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateMunicipality(
        string stateId,
        [FromBody] CreateMunicipalityDto createMunicipalityDto)
    {
        try
        {
            // Set the department ID in the DTO
            createMunicipalityDto.DepartmentId = stateId;
            
            var municipality = await _createMunicipalityUseCase.ExecuteAsync(createMunicipalityDto);
            return CreatedAtAction(nameof(CreateMunicipality), new { stateId = stateId, id = municipality.Id }, municipality);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new locality within a municipality
    /// </summary>
    /// <param name="stateId">State/Department ID</param>
    /// <param name="createLocalityDto">Locality data</param>
    /// <returns>Created locality</returns>
    /// <response code="201">Locality created successfully</response>
    /// <response code="400">Validation error</response>
    [HttpPost("localities")]
    [RequirePermission("administrar_parametros")]
    [ProducesResponseType(typeof(LocalityDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateLocality(
        string stateId,
        [FromBody] CreateLocalityDto createLocalityDto)
    {
        try
        {
            var locality = await _createLocalityUseCase.ExecuteAsync(createLocalityDto);
            return CreatedAtAction(nameof(CreateLocality), new { stateId = stateId, id = locality.Id }, locality);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new city (alternative endpoint)
    /// </summary>
    /// <param name="stateId">State/Department ID</param>
    /// <param name="createCityDto">City data</param>
    /// <returns>Created city</returns>
    /// <response code="201">City created successfully</response>
    /// <response code="400">Validation error</response>
    [HttpPost("alternative")]
    [RequirePermission("administrar_parametros")]
    [ProducesResponseType(typeof(CityDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateCity(
        string stateId,
        [FromBody] CreateCityDto createCityDto)
    {
        try
        {
            var city = await _createCityUseCase.ExecuteAsync(createCityDto);
            return CreatedAtAction(nameof(CreateCity), new { stateId = stateId, id = city.Id }, city);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
