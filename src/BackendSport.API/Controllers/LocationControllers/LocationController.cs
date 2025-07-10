using Microsoft.AspNetCore.Mvc;
using BackendSport.Application.DTOs.LocationDTOs;
using BackendSport.Application.UseCases.LocationUseCases;
using BackendSport.API.Attributes;

namespace BackendSport.API.Controllers.LocationControllers;

[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    private readonly GetLocationHierarchyUseCase _getLocationHierarchyUseCase;
    private readonly GetDocumentTypesByCountryUseCase _getDocumentTypesByCountryUseCase;
    private readonly CreateCountryUseCase _createCountryUseCase;
    private readonly CreateDepartmentUseCase _createDepartmentUseCase;
    private readonly CreateMunicipalityUseCase _createMunicipalityUseCase;
    private readonly CreateLocalityUseCase _createLocalityUseCase;
    private readonly CreateDocumentTypeUseCase _createDocumentTypeUseCase;

    public LocationController(
        GetLocationHierarchyUseCase getLocationHierarchyUseCase,
        GetDocumentTypesByCountryUseCase getDocumentTypesByCountryUseCase,
        CreateCountryUseCase createCountryUseCase,
        CreateDepartmentUseCase createDepartmentUseCase,
        CreateMunicipalityUseCase createMunicipalityUseCase,
        CreateLocalityUseCase createLocalityUseCase,
        CreateDocumentTypeUseCase createDocumentTypeUseCase)
    {
        _getLocationHierarchyUseCase = getLocationHierarchyUseCase;
        _getDocumentTypesByCountryUseCase = getDocumentTypesByCountryUseCase;
        _createCountryUseCase = createCountryUseCase;
        _createDepartmentUseCase = createDepartmentUseCase;
        _createMunicipalityUseCase = createMunicipalityUseCase;
        _createLocalityUseCase = createLocalityUseCase;
        _createDocumentTypeUseCase = createDocumentTypeUseCase;
    }

    /// <summary>
    /// Obtiene la jerarquía completa de ubicaciones para un país
    /// </summary>
    /// <param name="countryId">ID del país</param>
    /// <returns>Jerarquía de ubicaciones</returns>
    [HttpGet("hierarchy/{countryId}")]
    public async Task<ActionResult<LocationHierarchyResponseDto>> GetLocationHierarchy(string countryId)
    {
        try
        {
            var hierarchy = await _getLocationHierarchyUseCase.ExecuteAsync(countryId);
            return Ok(hierarchy);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene los tipos de documento disponibles para un país
    /// </summary>
    /// <param name="countryId">ID del país</param>
    /// <returns>Lista de tipos de documento</returns>
    [HttpGet("document-types/{countryId}")]
    public async Task<ActionResult<List<DocumentTypeDto>>> GetDocumentTypes(string countryId)
    {
        try
        {
            var documentTypes = await _getDocumentTypesByCountryUseCase.ExecuteAsync(countryId);
            return Ok(documentTypes);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Crea un nuevo país
    /// </summary>
    /// <param name="createCountryDto">Datos del país a crear</param>
    /// <returns>El país creado</returns>
    [HttpPost("countries")]
    [RequirePermission("administrar_parametros")]
    public async Task<ActionResult<CountryDto>> CreateCountry([FromBody] CreateCountryDto createCountryDto)
    {
        try
        {
            var country = await _createCountryUseCase.ExecuteAsync(createCountryDto);
            return CreatedAtAction(nameof(CreateCountry), new { id = country.Id }, country);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Crea un nuevo departamento
    /// </summary>
    /// <param name="createDepartmentDto">Datos del departamento a crear</param>
    /// <returns>El departamento creado</returns>
    [HttpPost("departments")]
    [RequirePermission("administrar_parametros")]
    public async Task<ActionResult<DepartmentDto>> CreateDepartment([FromBody] CreateDepartmentDto createDepartmentDto)
    {
        try
        {
            var department = await _createDepartmentUseCase.ExecuteAsync(createDepartmentDto);
            return CreatedAtAction(nameof(CreateDepartment), new { id = department.Id }, department);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Crea un nuevo municipio
    /// </summary>
    /// <param name="createMunicipalityDto">Datos del municipio a crear</param>
    /// <returns>El municipio creado</returns>
    [HttpPost("municipalities")]
    [RequirePermission("administrar_parametros")]
    public async Task<ActionResult<MunicipalityDto>> CreateMunicipality([FromBody] CreateMunicipalityDto createMunicipalityDto)
    {
        try
        {
            var municipality = await _createMunicipalityUseCase.ExecuteAsync(createMunicipalityDto);
            return CreatedAtAction(nameof(CreateMunicipality), new { id = municipality.Id }, municipality);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Crea una nueva localidad
    /// </summary>
    /// <param name="createLocalityDto">Datos de la localidad a crear</param>
    /// <returns>La localidad creada</returns>
    [HttpPost("localities")]
    [RequirePermission("administrar_parametros")]
    public async Task<ActionResult<LocalityDto>> CreateLocality([FromBody] CreateLocalityDto createLocalityDto)
    {
        try
        {
            var locality = await _createLocalityUseCase.ExecuteAsync(createLocalityDto);
            return CreatedAtAction(nameof(CreateLocality), new { id = locality.Id }, locality);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Crea un nuevo tipo de documento
    /// </summary>
    /// <param name="createDocumentTypeDto">Datos del tipo de documento a crear</param>
    /// <returns>El tipo de documento creado</returns>
    [HttpPost("document-types")]
    [RequirePermission("administrar_parametros")]
    public async Task<ActionResult<DocumentTypeDto>> CreateDocumentType([FromBody] CreateDocumentTypeDto createDocumentTypeDto)
    {
        try
        {
            var documentType = await _createDocumentTypeUseCase.ExecuteAsync(createDocumentTypeDto);
            return CreatedAtAction(nameof(CreateDocumentType), new { id = documentType.Id }, documentType);
        }
        catch (Exception ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
} 