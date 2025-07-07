using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackendSport.Domain.Entities.AuthEntities;

public class User
{
    [BsonId]
    public string Id { get; set; } = string.Empty;
    
    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;
    
    [BsonElement("password")]
    public string Password { get; set; } = string.Empty;

    [BsonElement("roleIds")]
    public List<string> RolIds { get; set; } = new List<string>();

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [BsonElement("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}