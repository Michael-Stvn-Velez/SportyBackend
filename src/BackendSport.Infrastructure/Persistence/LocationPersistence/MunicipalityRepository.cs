using MongoDB.Driver;
using BackendSport.Application.Interfaces.LocationInterfaces;
using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Infrastructure.Persistence.LocationPersistence;

public class MunicipalityRepository : IMunicipalityRepository
{
    private readonly IMongoCollection<Municipality> _municipalities;

    public MunicipalityRepository(MongoDbContext context)
    {
        _municipalities = context.Database.GetCollection<Municipality>("municipalities");
    }

    public async Task<List<Municipality>> GetAllAsync()
    {
        return await _municipalities.Find(m => m.IsActive).ToListAsync();
    }

    public async Task<List<Municipality>> GetByDepartmentIdAsync(string departmentId)
    {
        return await _municipalities.Find(m => m.DepartmentId == departmentId && m.IsActive).ToListAsync();
    }

    public async Task<Municipality?> GetByIdAsync(string id)
    {
        return await _municipalities.Find(m => m.Id == id && m.IsActive).FirstOrDefaultAsync();
    }

    public async Task<Municipality?> GetByCodeAsync(string code)
    {
        return await _municipalities.Find(m => m.Code == code && m.IsActive).FirstOrDefaultAsync();
    }

    public async Task<Municipality> CreateAsync(Municipality municipality)
    {
        await _municipalities.InsertOneAsync(municipality);
        return municipality;
    }
} 