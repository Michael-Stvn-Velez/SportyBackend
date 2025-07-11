using BackendSport.Application.DTOs.LocationDTOs;
using BackendSport.Application.Interfaces.LocationInterfaces;
using BackendSport.Domain.Entities.LocationEntities;

namespace BackendSport.Application.UseCases.LocationUseCases;

public class CreateCityUseCase
{
    private readonly ICityRepository _cityRepository;

    public CreateCityUseCase(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<CityDto> ExecuteAsync(CreateCityDto createCityDto)
    {
        var city = new City
        {
            Id = Guid.NewGuid().ToString(),
            Name = createCityDto.Name,
            Code = createCityDto.Code,
            MunicipalityId = createCityDto.MunicipalityId,
            Type = createCityDto.Type,
            IsCapital = createCityDto.IsCapital,
            IsActive = true
        };

        var createdCity = await _cityRepository.CreateAsync(city);

        return new CityDto
        {
            Id = createdCity.Id,
            Name = createdCity.Name,
            Code = createdCity.Code,
            MunicipalityId = createdCity.MunicipalityId,
            Type = createdCity.Type,
            IsCapital = createdCity.IsCapital
        };
    }
} 