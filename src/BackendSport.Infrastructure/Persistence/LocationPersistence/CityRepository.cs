using MongoDB.Driver;
using BackendSport.Application.Interfaces.LocationInterfaces;
using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Infrastructure.Persistence.LocationPersistence;

public class CityRepository : ICityRepository
{
    private readonly IMongoCollection<City> _cities;

    public CityRepository(MongoDbContext context)
    {
        _cities = context.Database.GetCollection<City>("cities");
    }

    public async Task<List<City>> GetAllAsync()
    {
        return await _cities.Find(c => c.IsActive).ToListAsync();
    }

    public async Task<List<City>> GetByMunicipalityIdAsync(string municipalityId)
    {
        return await _cities.Find(c => c.MunicipalityId == municipalityId && c.IsActive).ToListAsync();
    }

    public async Task<City?> GetByIdAsync(string id)
    {
        return await _cities.Find(c => c.Id == id && c.IsActive).FirstOrDefaultAsync();
    }

    public async Task<City?> GetByCodeAsync(string code)
    {
        return await _cities.Find(c => c.Code == code && c.IsActive).FirstOrDefaultAsync();
    }

    public async Task<City> CreateAsync(City city)
    {
        await _cities.InsertOneAsync(city);
        return city;
    }
} 