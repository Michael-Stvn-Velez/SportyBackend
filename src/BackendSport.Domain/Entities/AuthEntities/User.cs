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

    // Método para agregar deporte al usuario (solo ID)
    public bool AddSport(string sportId)
    {
        if (Sports.Count >= 3)
        {
            return false;
        }

        if (Sports.Any(s => s.SportId == sportId))
        {
            return false;
        }

        Sports.Add(new UserSport { SportId = sportId });
        UpdatedAt = DateTime.UtcNow;
        return true;
    }

    // Método para configurar deporte del usuario
    public bool ConfigureSport(string sportId, List<string> positions, string level, Dictionary<string, int> performanceMetrics)
    {
        var sport = Sports.FirstOrDefault(s => s.SportId == sportId);
        if (sport == null)
        {
            return false;
        }

        sport.Positions = positions;
        sport.Level = level;
        sport.PerformanceMetrics = performanceMetrics;
        sport.ConfiguredAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        return true;
    }

    // Método para verificar si tiene un deporte específico
    public bool HasSport(string sportId)
    {
        return Sports.Any(s => s.SportId == sportId);
    }

    // Método para verificar si un deporte está configurado
    public bool IsSportConfigured(string sportId)
    {
        var sport = Sports.FirstOrDefault(s => s.SportId == sportId);
        return sport?.ConfiguredAt != null;
    }

    // Método para obtener la cantidad de deportes
    public int GetSportsCount()
    {
        return Sports.Count;
    }
}