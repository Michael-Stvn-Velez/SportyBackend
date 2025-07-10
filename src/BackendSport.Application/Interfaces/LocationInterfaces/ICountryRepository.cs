using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Application.Interfaces.LocationInterfaces;

public interface ICountryRepository
{
    Task<List<Country>> GetAllAsync();
    Task<Country?> GetByIdAsync(string id);
    Task<Country?> GetByCodeAsync(string code);
    Task<Country> CreateAsync(Country country);
} 