using BackendSport.Domain.Entities.DeporteEntities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendSport.Application.Interfaces.DeporteInterfaces
{
    public interface IDeporteRepository
    {
        Task<List<Deporte>> GetAllAsync();
        Task<Deporte?> GetByIdAsync(string id);
        Task<Deporte?> GetByNombreAsync(string nombre);
        Task AddAsync(Deporte deporte);
        Task UpdateAsync(string id, Deporte deporte);
        Task DeleteAsync(string id);
        Task<bool> ExistsByNombreAsync(string nombre);
    }
} 