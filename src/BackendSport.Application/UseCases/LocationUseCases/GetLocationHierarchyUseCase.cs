using BackendSport.Application.DTOs.LocationDTOs;
using BackendSport.Application.Interfaces.LocationInterfaces;

namespace BackendSport.Application.UseCases.LocationUseCases;

public class GetLocationHierarchyUseCase
{
    private readonly ILocationHierarchyRepository _locationHierarchyRepository;

    public GetLocationHierarchyUseCase(ILocationHierarchyRepository locationHierarchyRepository)
    {
        _locationHierarchyRepository = locationHierarchyRepository;
    }

    public async Task<LocationHierarchyResponseDto> ExecuteAsync(string countryId)
    {
        return await _locationHierarchyRepository.GetLocationHierarchyAsync(countryId);
    }
} 