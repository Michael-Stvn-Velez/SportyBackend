using BackendSport.Application.Interfaces.DeporteInterfaces;
using BackendSport.Domain.Entities.DeporteEntities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendSport.Infrastructure.Persistence.DeportePersistence
{
    public class DeporteRepository : IDeporteRepository
    {
        private readonly IMongoCollection<Deporte> _deportes;

        public DeporteRepository(MongoDbContext context)
        {
            _deportes = context.Database.GetCollection<Deporte>("deportes");
        }

        public async Task<List<Deporte>> GetAllAsync()
        {
            return await _deportes.Find(_ => true).ToListAsync();
        }

        public async Task<Deporte?> GetByIdAsync(string id)
        {
            return await _deportes.Find(d => d.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Deporte?> GetByNombreAsync(string nombre)
        {
            return await _deportes.Find(d => d.Nombre == nombre).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Deporte deporte)
        {
            await _deportes.InsertOneAsync(deporte);
        }

        public async Task UpdateAsync(string id, Deporte deporte)
        {
            await _deportes.ReplaceOneAsync(d => d.Id == id, deporte);
        }

        public async Task DeleteAsync(string id)
        {
            await _deportes.DeleteOneAsync(d => d.Id == id);
        }

        public async Task<bool> ExistsByNombreAsync(string nombre)
        {
            return await _deportes.Find(d => d.Nombre == nombre).AnyAsync();
        }
    }
} 