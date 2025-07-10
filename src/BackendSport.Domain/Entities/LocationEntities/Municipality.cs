using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackendSport.Domain.Entities.LocationEntities;

public class Municipality
{
    [BsonId]
    public string Id { get; set; } = string.Empty;
    
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    
    [BsonElement("code")]
    public string Code { get; set; } = string.Empty;
    
    [BsonElement("departmentId")]
    public string DepartmentId { get; set; } = string.Empty;
    
    [BsonElement("type")]
    public string Type { get; set; } = string.Empty; // "Municipio", "Distrito", "Área Metropolitana"
    
    [BsonElement("isCapital")]
    public bool IsCapital { get; set; } = false;
    
    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;
} 