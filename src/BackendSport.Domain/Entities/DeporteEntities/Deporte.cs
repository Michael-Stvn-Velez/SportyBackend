using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace BackendSport.Domain.Entities.DeporteEntities
{
    public class Deporte
    {
        [BsonId]
        [BsonElement("_id")]
        public required string Id { get; set; }
        public required string Nombre { get; set; }
        public List<string>? Modalidad { get; set; } = new List<string>();
        public List<string>? Superficie { get; set; } = new List<string>();
        public List<string>? Posiciones { get; set; } = new List<string>();
        public List<string>? Estadisticas { get; set; } = new List<string>();
        public List<string>? MetricasRendimiento { get; set; } = new List<string>();
        public List<string>? TipoEvaluaciones { get; set; } = new List<string>();
        public List<string>? Formaciones { get; set; } = new List<string>();
        public List<string>? NivelCompetitivo { get; set; } = new List<string>();
    }
} 