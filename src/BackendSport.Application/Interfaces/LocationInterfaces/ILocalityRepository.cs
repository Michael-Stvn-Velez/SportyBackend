using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Application.Interfaces.LocationInterfaces;

public interface ILocalityRepository
{
    Task<List<Locality>> GetAllAsync();
    Task<List<Locality>> GetByMunicipalityIdAsync(string municipalityId);
    Task<Locality?> GetByIdAsync(string id);
    Task<Locality> CreateAsync(Locality locality);
} 