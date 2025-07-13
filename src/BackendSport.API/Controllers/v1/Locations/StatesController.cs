using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.DTOs.LocationDTOs;
using BackendSport.Application.UseCases.LocationUseCases;
using BackendSport.API.Attributes;

namespace BackendSport.API.Controllers.v1.Locations;

/// <summary>
/// Controller for states/departments management
/// </summary>
[ApiController]
[Route("api/v1/countries/{countryId}/states")]
[Produces("application/json")]
public class StatesController : ControllerBase
{
    private readonly CreateDepartmentUseCase _createDepartmentUseCase;

    public StatesController(CreateDepartmentUseCase createDepartmentUseCase)
    {
        _createDepartmentUseCase = createDepartmentUseCase;
    }

    /// <summary>
    /// Create a new state/department
    /// </summary>
    /// <param name="countryId">Country ID</param>
    /// <param name="createDepartmentDto">Department data</param>
    /// <returns>Created department</returns>
    /// <response code="201">Department created successfully</response>
    /// <response code="400">Validation error</response>
    [HttpPost]
    [RequirePermission("administrar_parametros")]
    [ProducesResponseType(typeof(DepartmentDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateState(
        string countryId,
        [FromBody] CreateDepartmentDto createDepartmentDto)
    {
        try
        {
            // Set the country ID in the DTO
            createDepartmentDto.CountryId = countryId;
            
            var department = await _createDepartmentUseCase.ExecuteAsync(createDepartmentDto);
            return CreatedAtAction(nameof(CreateState), new { countryId = countryId, id = department.Id }, department);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
