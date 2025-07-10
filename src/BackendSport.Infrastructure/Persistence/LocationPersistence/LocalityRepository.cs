using MongoDB.Driver;
using BackendSport.Application.Interfaces.LocationInterfaces;
using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Infrastructure.Persistence.LocationPersistence;

public class LocalityRepository : ILocalityRepository
{
    private readonly IMongoCollection<Locality> _localities;

    public LocalityRepository(MongoDbContext context)
    {
        _localities = context.Database.GetCollection<Locality>("localities");
    }

    public async Task<List<Locality>> GetAllAsync()
    {
        return await _localities.Find(l => l.IsActive).ToListAsync();
    }

    public async Task<List<Locality>> GetByMunicipalityIdAsync(string municipalityId)
    {
        return await _localities.Find(l => l.MunicipalityId == municipalityId && l.IsActive).ToListAsync();
    }

    public async Task<Locality?> GetByIdAsync(string id)
    {
        return await _localities.Find(l => l.Id == id && l.IsActive).FirstOrDefaultAsync();
    }

    public async Task<Locality> CreateAsync(Locality locality)
    {
        await _localities.InsertOneAsync(locality);
        return locality;
    }
} 