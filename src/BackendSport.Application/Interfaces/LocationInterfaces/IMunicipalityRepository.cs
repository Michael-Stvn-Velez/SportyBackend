using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Application.Interfaces.LocationInterfaces;

public interface IMunicipalityRepository
{
    Task<List<Municipality>> GetAllAsync();
    Task<List<Municipality>> GetByDepartmentIdAsync(string departmentId);
    Task<Municipality?> GetByIdAsync(string id);
    Task<Municipality?> GetByCodeAsync(string code);
    Task<Municipality> CreateAsync(Municipality municipality);
} 