using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackendSport.Domain.Entities.LocationEntities;

public class City
{
    [BsonId]
    public string Id { get; set; } = string.Empty;
    
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    
    [BsonElement("code")]
    public string Code { get; set; } = string.Empty;
    
    [BsonElement("municipalityId")]
    public string MunicipalityId { get; set; } = string.Empty;
    
    [BsonElement("type")]
    public string Type { get; set; } = string.Empty; // "Ciudad", "Pueblo", "Villa"
    
    [BsonElement("isCapital")]
    public bool IsCapital { get; set; } = false;
    
    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;
} 