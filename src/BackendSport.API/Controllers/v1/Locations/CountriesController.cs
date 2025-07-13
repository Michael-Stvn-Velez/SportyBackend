using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.DTOs.LocationDTOs;
using BackendSport.Application.UseCases.LocationUseCases;
using BackendSport.API.Attributes;

namespace BackendSport.API.Controllers.v1.Locations;

/// <summary>
/// Controller for countries management
/// </summary>
[ApiController]
[Route("api/v1/countries")]
[Produces("application/json")]
public class CountriesController : ControllerBase
{
    private readonly CreateCountryUseCase _createCountryUseCase;
    private readonly GetLocationHierarchyUseCase _getLocationHierarchyUseCase;
    private readonly GetDocumentTypesByCountryUseCase _getDocumentTypesByCountryUseCase;
    private readonly CreateDocumentTypeUseCase _createDocumentTypeUseCase;

    public CountriesController(
        CreateCountryUseCase createCountryUseCase,
        GetLocationHierarchyUseCase getLocationHierarchyUseCase,
        GetDocumentTypesByCountryUseCase getDocumentTypesByCountryUseCase,
        CreateDocumentTypeUseCase createDocumentTypeUseCase)
    {
        _createCountryUseCase = createCountryUseCase;
        _getLocationHierarchyUseCase = getLocationHierarchyUseCase;
        _getDocumentTypesByCountryUseCase = getDocumentTypesByCountryUseCase;
        _createDocumentTypeUseCase = createDocumentTypeUseCase;
    }

    /// <summary>
    /// Create a new country
    /// </summary>
    /// <param name="createCountryDto">Country data</param>
    /// <returns>Created country</returns>
    /// <response code="201">Country created successfully</response>
    /// <response code="400">Validation error</response>
    [HttpPost]
    [RequirePermission("administrar_parametros")]
    [ProducesResponseType(typeof(CountryDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDto createCountryDto)
    {
        try
        {
            var country = await _createCountryUseCase.ExecuteAsync(createCountryDto);
            return CreatedAtAction(nameof(GetCountryHierarchy), new { id = country.Id }, country);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get complete location hierarchy for a country
    /// </summary>
    /// <param name="id">Country ID</param>
    /// <returns>Location hierarchy</returns>
    /// <response code="200">Hierarchy retrieved successfully</response>
    /// <response code="400">Error retrieving hierarchy</response>
    [HttpGet("{id}/hierarchy")]
    [ProducesResponseType(typeof(LocationHierarchyResponseDto), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetCountryHierarchy(string id)
    {
        try
        {
            var hierarchy = await _getLocationHierarchyUseCase.ExecuteAsync(id);
            return Ok(hierarchy);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get document types available for a country
    /// </summary>
    /// <param name="id">Country ID</param>
    /// <returns>List of document types</returns>
    /// <response code="200">Document types retrieved successfully</response>
    /// <response code="400">Error retrieving document types</response>
    [HttpGet("{id}/document-types")]
    [ProducesResponseType(typeof(List<DocumentTypeDto>), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetDocumentTypes(string id)
    {
        try
        {
            var documentTypes = await _getDocumentTypesByCountryUseCase.ExecuteAsync(id);
            return Ok(documentTypes);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Create a new document type for a country
    /// </summary>
    /// <param name="id">Country ID</param>
    /// <param name="createDocumentTypeDto">Document type data</param>
    /// <returns>Created document type</returns>
    /// <response code="201">Document type created successfully</response>
    /// <response code="400">Validation error</response>
    [HttpPost("{id}/document-types")]
    [RequirePermission("administrar_parametros")]
    [ProducesResponseType(typeof(DocumentTypeDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateDocumentType(
        string id, 
        [FromBody] CreateDocumentTypeDto createDocumentTypeDto)
    {
        try
        {
            // Set the country ID in the DTO
            createDocumentTypeDto.CountryId = id;
            
            var documentType = await _createDocumentTypeUseCase.ExecuteAsync(createDocumentTypeDto);
            return CreatedAtAction(nameof(GetDocumentTypes), new { id = id }, documentType);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
