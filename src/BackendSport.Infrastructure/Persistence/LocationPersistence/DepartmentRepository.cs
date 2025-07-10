using MongoDB.Driver;
using BackendSport.Application.Interfaces.LocationInterfaces;
using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Infrastructure.Persistence.LocationPersistence;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly IMongoCollection<Department> _departments;

    public DepartmentRepository(MongoDbContext context)
    {
        _departments = context.Database.GetCollection<Department>("departments");
    }

    public async Task<List<Department>> GetAllAsync()
    {
        return await _departments.Find(d => d.IsActive).ToListAsync();
    }

    public async Task<List<Department>> GetByCountryIdAsync(string countryId)
    {
        return await _departments.Find(d => d.CountryId == countryId && d.IsActive).ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(string id)
    {
        return await _departments.Find(d => d.Id == id && d.IsActive).FirstOrDefaultAsync();
    }

    public async Task<Department?> GetByCodeAsync(string code)
    {
        return await _departments.Find(d => d.Code == code && d.IsActive).FirstOrDefaultAsync();
    }

    public async Task<Department> CreateAsync(Department department)
    {
        await _departments.InsertOneAsync(department);
        return department;
    }
} 