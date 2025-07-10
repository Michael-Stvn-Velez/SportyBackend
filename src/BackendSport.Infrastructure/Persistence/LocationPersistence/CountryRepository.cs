using MongoDB.Driver;
using BackendSport.Application.Interfaces.LocationInterfaces;
using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Infrastructure.Persistence.LocationPersistence;

public class CountryRepository : ICountryRepository
{
    private readonly IMongoCollection<Country> _countries;

    public CountryRepository(MongoDbContext context)
    {
        _countries = context.Database.GetCollection<Country>("countries");
    }

    public async Task<List<Country>> GetAllAsync()
    {
        return await _countries.Find(c => c.IsActive).ToListAsync();
    }

    public async Task<Country?> GetByIdAsync(string id)
    {
        return await _countries.Find(c => c.Id == id && c.IsActive).FirstOrDefaultAsync();
    }

    public async Task<Country?> GetByCodeAsync(string code)
    {
        return await _countries.Find(c => c.Code == code && c.IsActive).FirstOrDefaultAsync();
    }

    public async Task<Country> CreateAsync(Country country)
    {
        await _countries.InsertOneAsync(country);
        return country;
    }
} 