using BackendSport.Application.Interfaces.LocationInterfaces;
using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.API.GraphQL.Queries;

/// <summary>
/// Consultas GraphQL para entidades de ubicación
/// </summary>
[ExtendObjectType("Query")]
public class LocationQueries
{
    /// <summary>
    /// Obtiene todos los países
    /// </summary>
    /// <param name="countryRepository">Repositorio de países</param>
    /// <returns>Lista de países</returns>
    public async Task<IEnumerable<Country>> GetCountriesAsync([Service] ICountryRepository countryRepository)
    {
        return await countryRepository.GetAllAsync();
    }

    /// <summary>
    /// Obtiene un país por su ID
    /// </summary>
    /// <param name="id">ID del país</param>
    /// <param name="countryRepository">Repositorio de países</param>
    /// <returns>País encontrado o null</returns>
    public async Task<Country?> GetCountryByIdAsync(
        string id, 
        [Service] ICountryRepository countryRepository)
    {
        return await countryRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Obtiene departamentos por país
    /// </summary>
    /// <param name="countryId">ID del país</param>
    /// <param name="departmentRepository">Repositorio de departamentos</param>
    /// <returns>Lista de departamentos</returns>
    public async Task<IEnumerable<Department>> GetDepartmentsByCountryAsync(
        string countryId,
        [Service] IDepartmentRepository departmentRepository)
    {
        return await departmentRepository.GetByCountryIdAsync(countryId);
    }

    /// <summary>
    /// Obtiene municipios por departamento
    /// </summary>
    /// <param name="departmentId">ID del departamento</param>
    /// <param name="municipalityRepository">Repositorio de municipios</param>
    /// <returns>Lista de municipios</returns>
    public async Task<IEnumerable<Municipality>> GetMunicipalitiesByDepartmentAsync(
        string departmentId,
        [Service] IMunicipalityRepository municipalityRepository)
    {
        return await municipalityRepository.GetByDepartmentIdAsync(departmentId);
    }

    /// <summary>
    /// Obtiene tipos de documento por país
    /// </summary>
    /// <param name="countryId">ID del país</param>
    /// <param name="documentTypeRepository">Repositorio de tipos de documento</param>
    /// <returns>Lista de tipos de documento</returns>
    public async Task<IEnumerable<DocumentType>> GetDocumentTypesByCountryAsync(
        string countryId,
        [Service] IDocumentTypeRepository documentTypeRepository)
    {
        return await documentTypeRepository.GetByCountryIdAsync(countryId);
    }

    /// <summary>
    /// Busca países por nombre
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda</param>
    /// <param name="countryRepository">Repositorio de países</param>
    /// <returns>Lista de países que coinciden con la búsqueda</returns>
    public async Task<IEnumerable<Country>> SearchCountriesAsync(
        string searchTerm,
        [Service] ICountryRepository countryRepository)
    {
        var allCountries = await countryRepository.GetAllAsync();
        return allCountries.Where(c => 
            c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            c.Code.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }
}
