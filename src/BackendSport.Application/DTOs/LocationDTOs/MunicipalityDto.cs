namespace BackendSport.Application.DTOs.LocationDTOs;

public class MunicipalityDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string DepartmentId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsCapital { get; set; }
} 