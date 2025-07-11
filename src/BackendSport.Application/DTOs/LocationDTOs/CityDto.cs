namespace BackendSport.Application.DTOs.LocationDTOs;

public class CityDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string MunicipalityId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsCapital { get; set; }
} 