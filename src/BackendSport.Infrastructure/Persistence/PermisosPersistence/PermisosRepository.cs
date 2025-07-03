using BackendSport.Application.Interfaces.PermisosInterfaces;
using BackendSport.Domain.Entities.PermisosEntities;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace BackendSport.Infrastructure.Persistence.PermisosPersistence
{
    public class PermisosRepository : IPermisosRepository
    {
        private readonly IMongoCollection<Permisos> _permisos;

        public PermisosRepository(MongoDbContext context)
        {
            _permisos = context.Database.GetCollection<Permisos>("permisos");
        }

        public async Task<Permisos> CrearPermisosAsync(Permisos permisos)
        {
            await _permisos.InsertOneAsync(permisos);
            return permisos;
        }

        public async Task<bool> ExisteNombreAsync(string nombre)
        {
            var filter = Builders<Permisos>.Filter.Eq(p => p.Nombre, nombre);
            return await _permisos.Find(filter).AnyAsync();
        }

        public async Task<Permisos> ObtenerPorIdAsync(string id)
        {
            var filter = Builders<Permisos>.Filter.Eq(p => p.Id, id);
            return await _permisos.Find(filter).FirstOrDefaultAsync();
        }
        public async Task ActualizarPermisosAsync(Permisos permisos)
        {
            var filter = Builders<Permisos>.Filter.Eq(p => p.Id, permisos.Id);
            await _permisos.ReplaceOneAsync(filter, permisos);
        }
        public async Task EliminarPermisosAsync(string id)
        {
            var filter = Builders<Permisos>.Filter.Eq(p => p.Id, id);
            await _permisos.DeleteOneAsync(filter);
        }
        public async Task<List<Permisos>> ObtenerTodosAsync()
        {
            return await _permisos.Find(_ => true).ToListAsync();
        }
    }
} 