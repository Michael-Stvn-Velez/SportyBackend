using System.ComponentModel.DataAnnotations;

namespace BackendSport.Application.DTOs.DeporteDTOs;

public class DeporteConfigOptionsDto
{
    public string DeporteId { get; set; } = string.Empty;
    public string DeporteNombre { get; set; } = string.Empty;
    public List<string> Posiciones { get; set; } = new List<string>();
    public List<string> Niveles { get; set; } = new List<string>();
    public List<string> MetricasRendimiento { get; set; } = new List<string>();
}

public class ConfigureUserSportDto
{
    [Required(ErrorMessage = "Las posiciones son requeridas")]
    public List<string> Positions { get; set; } = new List<string>();

    [Required(ErrorMessage = "El nivel es requerido")]
    public string Level { get; set; } = string.Empty;

    [Required(ErrorMessage = "Las métricas de rendimiento son requeridas")]
    public Dictionary<string, int> PerformanceMetrics { get; set; } = new Dictionary<string, int>();
}

public class UserSportConfigDto
{
    public string SportId { get; set; } = string.Empty;
    public string SportName { get; set; } = string.Empty;
    public List<string> Positions { get; set; } = new List<string>();
    public string Level { get; set; } = string.Empty;
    public Dictionary<string, int> PerformanceMetrics { get; set; } = new Dictionary<string, int>();
    public DateTime? ConfiguredAt { get; set; }
} 