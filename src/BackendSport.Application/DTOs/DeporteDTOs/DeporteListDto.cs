using System.Collections.Generic;

namespace BackendSport.Application.DTOs.DeporteDTOs
{
    public class DeporteListDto
    {
        public required string Id { get; set; }
        public required string Nombre { get; set; }
        public List<string>? Modalidad { get; set; }
        public List<string>? Superficie { get; set; }
        public List<string>? Posiciones { get; set; }
        public List<string>? Estadisticas { get; set; }
        public List<string>? MetricasRendimiento { get; set; }
        public List<string>? TipoEvaluaciones { get; set; }
        public List<string>? Formaciones { get; set; }
        public List<string>? NivelCompetitivo { get; set; }
    }
} 