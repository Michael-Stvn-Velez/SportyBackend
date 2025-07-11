using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Application.Interfaces.LocationInterfaces;

public interface ICityRepository
{
    Task<List<City>> GetAllAsync();
    Task<List<City>> GetByMunicipalityIdAsync(string municipalityId);
    Task<City?> GetByIdAsync(string id);
    Task<City?> GetByCodeAsync(string code);
    Task<City> CreateAsync(City city);
} 