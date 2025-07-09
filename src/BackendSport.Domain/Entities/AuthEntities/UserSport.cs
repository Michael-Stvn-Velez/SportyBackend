using MongoDB.Bson.Serialization.Attributes;

namespace BackendSport.Domain.Entities.AuthEntities;

public class UserSport
{
    [BsonElement("sportId")]
    public string SportId { get; set; } = string.Empty;

    [BsonElement("positions")]
    public List<string> Positions { get; set; } = new List<string>();

    [BsonElement("level")]
    public string Level { get; set; } = string.Empty;

    [BsonElement("performanceMetrics")]
    public Dictionary<string, int> PerformanceMetrics { get; set; } = new Dictionary<string, int>();

    [BsonElement("configuredAt")]
    public DateTime? ConfiguredAt { get; set; }
} 