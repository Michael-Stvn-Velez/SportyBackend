using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackendSport.Domain.Entities.AuthEntities;

public class User
{
    [BsonId]
    public string Id { get; set; } = string.Empty;
    
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    
    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;
    // Documento
    [BsonElement("documentTypeId")]
    public string DocumentTypeId { get; set; } = string.Empty;
    
    [BsonElement("documentNumber")]
    public string DocumentNumber { get; set; } = string.Empty;
    
    [BsonElement("countryId")]
    public string CountryId { get; set; } = string.Empty;
    
    [BsonElement("departmentId")]
    public string? DepartmentId { get; set; } = null;
    
    [BsonElement("municipalityId")]
    public string? MunicipalityId { get; set; } = null;
    
    [BsonElement("localityId")]
    public string? LocalityId { get; set; } = null;

    [BsonElement("password")]
    public string Password { get; set; } = string.Empty;

    [BsonElement("roleIds")]
    public List<string> RolIds { get; set; } = new List<string>();

    [BsonElement("sports")]
    public List<UserSport> Sports { get; set; } = new List<UserSport>();

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [BsonElement("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}
