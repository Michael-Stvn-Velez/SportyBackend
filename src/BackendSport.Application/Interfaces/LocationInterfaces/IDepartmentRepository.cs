using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Application.Interfaces.LocationInterfaces;

public interface IDepartmentRepository
{
    Task<List<Department>> GetAllAsync();
    Task<List<Department>> GetByCountryIdAsync(string countryId);
    Task<Department?> GetByIdAsync(string id);
    Task<Department?> GetByCodeAsync(string code);
    Task<Department> CreateAsync(Department department);
} 