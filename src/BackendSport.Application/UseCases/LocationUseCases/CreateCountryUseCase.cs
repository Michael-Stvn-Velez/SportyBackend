using BackendSport.Application.DTOs.LocationDTOs;
using BackendSport.Application.Interfaces.LocationInterfaces;
using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Application.UseCases.LocationUseCases;

public class CreateCountryUseCase
{
    private readonly ICountryRepository _countryRepository;

    public CreateCountryUseCase(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public async Task<CountryDto> ExecuteAsync(CreateCountryDto createCountryDto)
    {
        // Validar que el código no esté duplicado
        var existingCountry = await _countryRepository.GetByCodeAsync(createCountryDto.Code);
        if (existingCountry != null)
        {
            throw new InvalidOperationException($"Ya existe un país con el código {createCountryDto.Code}");
        }

        var country = new Country
        {
            Id = Guid.NewGuid().ToString(),
            Name = createCountryDto.Name,
            Code = createCountryDto.Code,
            IsActive = true
        };

        var createdCountry = await _countryRepository.CreateAsync(country);

        return new CountryDto
        {
            Id = createdCountry.Id,
            Name = createdCountry.Name,
            Code = createdCountry.Code
        };
    }
} 