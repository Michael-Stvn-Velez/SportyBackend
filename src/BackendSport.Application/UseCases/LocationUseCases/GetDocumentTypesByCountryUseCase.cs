using BackendSport.Application.DTOs.LocationDTOs;
using BackendSport.Application.Interfaces.LocationInterfaces;

namespace BackendSport.Application.UseCases.LocationUseCases;

public class GetDocumentTypesByCountryUseCase
{
    private readonly IDocumentTypeRepository _documentTypeRepository;

    public GetDocumentTypesByCountryUseCase(IDocumentTypeRepository documentTypeRepository)
    {
        _documentTypeRepository = documentTypeRepository;
    }

    public async Task<List<DocumentTypeDto>> ExecuteAsync(string countryId)
    {
        var documentTypes = await _documentTypeRepository.GetByCountryIdAsync(countryId);
        
        return documentTypes
            .Where(dt => dt.IsActive)
            .Select(dt => new DocumentTypeDto
            {
                Id = dt.Id,
                Name = dt.Name,
                Code = dt.Code,
                CountryId = dt.CountryId
            })
            .ToList();
    }
} 