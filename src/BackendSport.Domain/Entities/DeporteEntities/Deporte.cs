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
        public required string Name { get; set; }
        public List<string>? Modalities { get; set; } = new List<string>();
        public List<string>? Surfaces { get; set; } = new List<string>();
        public List<string>? Positions { get; set; } = new List<string>();
        public List<string>? Statistics { get; set; } = new List<string>();
        public List<string>? PerformanceMetrics { get; set; } = new List<string>();
        public List<string>? EvaluationTypes { get; set; } = new List<string>();
        public List<string>? Formations { get; set; } = new List<string>();
        public List<string>? CompetitiveLevel { get; set; } = new List<string>();
    }
} 