using BackendSport.Application.DTOs.LocationDTOs;
using BackendSport.Application.Interfaces.LocationInterfaces;
using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Application.UseCases.LocationUseCases;

public class CreateLocalityUseCase
{
    private readonly ILocalityRepository _localityRepository;
    private readonly IMunicipalityRepository _municipalityRepository;

    public CreateLocalityUseCase(ILocalityRepository localityRepository, IMunicipalityRepository municipalityRepository)
    {
        _localityRepository = localityRepository;
        _municipalityRepository = municipalityRepository;
    }

    public async Task<LocalityDto> ExecuteAsync(CreateLocalityDto createLocalityDto)
    {
        // Validar que el municipio existe
        var municipality = await _municipalityRepository.GetByIdAsync(createLocalityDto.MunicipalityId);
        if (municipality == null)
        {
            throw new InvalidOperationException($"No existe un municipio con el ID {createLocalityDto.MunicipalityId}");
        }

        // Validar que el código no esté duplicado
        var existingLocality = await _localityRepository.GetByIdAsync(createLocalityDto.Code);
        if (existingLocality != null)
        {
            throw new InvalidOperationException($"Ya existe una localidad con el código {createLocalityDto.Code}");
        }

        var locality = new Locality
        {
            Id = Guid.NewGuid().ToString(),
            Name = createLocalityDto.Name,
            Code = createLocalityDto.Code,
            MunicipalityId = createLocalityDto.MunicipalityId,
            Type = createLocalityDto.Type,
            IsActive = true
        };

        var createdLocality = await _localityRepository.CreateAsync(locality);

        return new LocalityDto
        {
            Id = createdLocality.Id,
            Name = createdLocality.Name,
            Code = createdLocality.Code,
            MunicipalityId = createdLocality.MunicipalityId,
            Type = createdLocality.Type
        };
    }
} 