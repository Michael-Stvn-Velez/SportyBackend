using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Application.Interfaces.LocationInterfaces;

public interface IDocumentTypeRepository
{
    Task<List<DocumentType>> GetAllAsync();
    Task<List<DocumentType>> GetByCountryIdAsync(string countryId);
    Task<DocumentType?> GetByIdAsync(string id);
    Task<DocumentType?> GetByCodeAsync(string code);
    Task<DocumentType> CreateAsync(DocumentType documentType);
} 