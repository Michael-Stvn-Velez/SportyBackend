using BackendSport.Application.Interfaces.DeporteInterfaces;
using BackendSport.Domain.Entities.DeporteEntities;
using HotChocolate.Authorization;

namespace BackendSport.API.GraphQL.Queries;

/// <summary>
/// Consultas GraphQL para deportes
/// </summary>
[ExtendObjectType("Query")]
public class DeporteQueries
{
    /// <summary>
    /// Obtiene todos los deportes disponibles
    /// </summary>
    /// <param name="deporteRepository">Repositorio de deportes</param>
    /// <returns>Lista de deportes</returns>
    public async Task<IEnumerable<Deporte>> GetDeportesAsync([Service] IDeporteRepository deporteRepository)
    {
        return await deporteRepository.GetAllAsync();
    }

    /// <summary>
    /// Obtiene un deporte por su ID
    /// </summary>
    /// <param name="id">ID del deporte</param>
    /// <param name="deporteRepository">Repositorio de deportes</param>
    /// <returns>Deporte encontrado o null</returns>
    public async Task<Deporte?> GetDeporteByIdAsync(
        string id, 
        [Service] IDeporteRepository deporteRepository)
    {
        return await deporteRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Busca deportes por nombre
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda</param>
    /// <param name="deporteRepository">Repositorio de deportes</param>
    /// <returns>Lista de deportes que coinciden con la búsqueda</returns>
    public async Task<IEnumerable<Deporte>> SearchDeportesAsync(
        string searchTerm,
        [Service] IDeporteRepository deporteRepository)
    {
        var allDeportes = await deporteRepository.GetAllAsync();
        return allDeportes.Where(d => 
            d.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Obtiene deportes que tienen posiciones específicas
    /// </summary>
    /// <param name="position">Posición a buscar</param>
    /// <param name="deporteRepository">Repositorio de deportes</param>
    /// <returns>Lista de deportes con la posición especificada</returns>
    public async Task<IEnumerable<Deporte>> GetDeportesByPositionAsync(
        string position,
        [Service] IDeporteRepository deporteRepository)
    {
        var allDeportes = await deporteRepository.GetAllAsync();
        return allDeportes.Where(d => 
            d.Positions != null && d.Positions.Contains(position, StringComparer.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Obtiene deportes por modalidad
    /// </summary>
    /// <param name="modality">Modalidad a buscar</param>
    /// <param name="deporteRepository">Repositorio de deportes</param>
    /// <returns>Lista de deportes con la modalidad especificada</returns>
    public async Task<IEnumerable<Deporte>> GetDeportesByModalityAsync(
        string modality,
        [Service] IDeporteRepository deporteRepository)
    {
        var allDeportes = await deporteRepository.GetAllAsync();
        return allDeportes.Where(d => 
            d.Modalities != null && d.Modalities.Contains(modality, StringComparer.OrdinalIgnoreCase));
    }
}
