using MongoDB.Driver;
using BackendSport.Application.Interfaces.LocationInterfaces;
using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Infrastructure.Persistence.LocationPersistence;

public class DocumentTypeRepository : IDocumentTypeRepository
{
    private readonly IMongoCollection<DocumentType> _documentTypes;

    public DocumentTypeRepository(MongoDbContext context)
    {
        _documentTypes = context.Database.GetCollection<DocumentType>("documentTypes");
    }

    public async Task<List<DocumentType>> GetAllAsync()
    {
        return await _documentTypes.Find(dt => dt.IsActive).ToListAsync();
    }

    public async Task<List<DocumentType>> GetByCountryIdAsync(string countryId)
    {
        return await _documentTypes.Find(dt => dt.CountryId == countryId && dt.IsActive).ToListAsync();
    }

    public async Task<DocumentType?> GetByIdAsync(string id)
    {
        return await _documentTypes.Find(dt => dt.Id == id && dt.IsActive).FirstOrDefaultAsync();
    }

    public async Task<DocumentType?> GetByCodeAsync(string code)
    {
        return await _documentTypes.Find(dt => dt.Code == code && dt.IsActive).FirstOrDefaultAsync();
    }

    public async Task<DocumentType> CreateAsync(DocumentType documentType)
    {
        await _documentTypes.InsertOneAsync(documentType);
        return documentType;
    }
} 