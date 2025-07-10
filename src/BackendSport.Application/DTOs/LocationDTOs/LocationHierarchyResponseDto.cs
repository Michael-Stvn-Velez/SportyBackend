namespace BackendSport.Application.DTOs.LocationDTOs;

public class LocationHierarchyResponseDto
{
    public List<CountryDto> Countries { get; set; } = new();
    public List<DepartmentDto> Departments { get; set; } = new();
    public List<MunicipalityDto> Municipalities { get; set; } = new();
    public List<LocalityDto> Localities { get; set; } = new();
} 