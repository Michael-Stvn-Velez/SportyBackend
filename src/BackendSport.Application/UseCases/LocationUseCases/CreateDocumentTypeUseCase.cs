using BackendSport.Application.DTOs.LocationDTOs;
using BackendSport.Application.Interfaces.LocationInterfaces;
using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Application.UseCases.LocationUseCases;

public class CreateDocumentTypeUseCase
{
    private readonly IDocumentTypeRepository _documentTypeRepository;
    private readonly ICountryRepository _countryRepository;

    public CreateDocumentTypeUseCase(IDocumentTypeRepository documentTypeRepository, ICountryRepository countryRepository)
    {
        _documentTypeRepository = documentTypeRepository;
        _countryRepository = countryRepository;
    }

    public async Task<DocumentTypeDto> ExecuteAsync(CreateDocumentTypeDto createDocumentTypeDto)
    {
        // Validar que el país existe
        var country = await _countryRepository.GetByIdAsync(createDocumentTypeDto.CountryId);
        if (country == null)
        {
            throw new InvalidOperationException($"No existe un país con el ID {createDocumentTypeDto.CountryId}");
        }

        // Validar que el código no esté duplicado
        var existingDocumentType = await _documentTypeRepository.GetByCodeAsync(createDocumentTypeDto.Code);
        if (existingDocumentType != null)
        {
            throw new InvalidOperationException($"Ya existe un tipo de documento con el código {createDocumentTypeDto.Code}");
        }

        var documentType = new DocumentType
        {
            Id = Guid.NewGuid().ToString(),
            Name = createDocumentTypeDto.Name,
            Code = createDocumentTypeDto.Code,
            CountryId = createDocumentTypeDto.CountryId,
            IsActive = true
        };

        var createdDocumentType = await _documentTypeRepository.CreateAsync(documentType);

        return new DocumentTypeDto
        {
            Id = createdDocumentType.Id,
            Name = createdDocumentType.Name,
            Code = createdDocumentType.Code,
            CountryId = createdDocumentType.CountryId
        };
    }
} 