using BackendSport.Application.DTOs.LocationDTOs;

namespace BackendSport.Application.Interfaces.LocationInterfaces;

public interface ILocationHierarchyRepository
{
    Task<LocationHierarchyResponseDto> GetLocationHierarchyAsync(string countryId);
} 