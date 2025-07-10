using System.Collections.Generic;

namespace BackendSport.Application.DTOs.DeporteDTOs
{
    public class DeporteDto
    {
        public required string Name { get; set; }
        public List<string>? Modalities { get; set; }
        public List<string>? Surfaces { get; set; }
        public List<string>? Positions { get; set; }
        public List<string>? Statistics { get; set; }
        public List<string>? PerformanceMetrics { get; set; }
        public List<string>? EvaluationTypes { get; set; }
        public List<string>? Formations { get; set; }
        public List<string>? CompetitiveLevel { get; set; }
    }
} 