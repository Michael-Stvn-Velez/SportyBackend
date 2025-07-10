namespace BackendSport.Application.DTOs.LocationDTOs;

public class CreateLocalityDto
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string MunicipalityId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
} 