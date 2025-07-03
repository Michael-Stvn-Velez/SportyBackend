using BackendSport.Application.Interfaces.RolInterfaces;
using BackendSport.Domain.Entities.RolEntities;
using MongoDB.Driver;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BackendSport.Infrastructure.Persistence.RolPersistence
{
    public class RolRepository : IRolRepository
    {
        private readonly IMongoCollection<Rol> _roles;

        public RolRepository(MongoDbContext context)
        {
            _roles = context.Database.GetCollection<Rol>("roles");
        }

        public async Task<Rol> CrearRolAsync(Rol rol)
        {
            await _roles.InsertOneAsync(rol);
            return rol;
        }

        public async Task<bool> ExisteNombreAsync(string nombre)
        {
            var filter = Builders<Rol>.Filter.Eq(r => r.Nombre, nombre);
            return await _roles.Find(filter).AnyAsync();
        }

        public async Task<Rol> ObtenerPorIdAsync(string id)
        {
            var filter = Builders<Rol>.Filter.Eq(r => r.Id, id);
            return await _roles.Find(filter).FirstOrDefaultAsync();
        }

        public async Task ActualizarRolAsync(Rol rol)
        {
            var filter = Builders<Rol>.Filter.Eq(r => r.Id, rol.Id);
            await _roles.ReplaceOneAsync(filter, rol);
        }

        public async Task EliminarRolAsync(string id)
        {
            var filter = Builders<Rol>.Filter.Eq(r => r.Id, id);
            await _roles.DeleteOneAsync(filter);
        }

        public async Task<List<Rol>> ObtenerTodosAsync()
        {
            return await _roles.Find(_ => true).ToListAsync();
        }
    }
} 