using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BackendSport.Domain.Entities.AuthEntities
{
    public class RefreshToken
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [BsonElement("userId")]
        public string UserId { get; set; } = string.Empty;

        [BsonElement("tokenHash")]
        public string TokenHash { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("expiresAt")]
        public DateTime ExpiresAt { get; set; }

        [BsonElement("revoked")]
        public bool Revoked { get; set; } = false;

        [BsonElement("tokenId")]
        public string TokenId { get; set; } = string.Empty;
    }
} 