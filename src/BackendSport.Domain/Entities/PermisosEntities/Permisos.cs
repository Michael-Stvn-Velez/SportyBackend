using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace BackendSport.Domain.Entities.PermisosEntities
{
    public class Permisos
    {
        [BsonId]
        [BsonElement("_id")]
        public required string Id { get; set; }
        public required string Nombre { get; set; }
        public required string Descripcion { get; set; }
    }
}