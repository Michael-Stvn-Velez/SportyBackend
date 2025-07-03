using BackendSport.Domain.Entities.PermisosEntities;
using System.Threading.Tasks;

namespace BackendSport.Application.Interfaces.PermisosInterfaces
{
    public interface IPermisosRepository
    {
        Task<Permisos> CrearPermisosAsync(Permisos permisos);
        Task<bool> ExisteNombreAsync(string nombre);
        Task<Permisos> ObtenerPorIdAsync(string id);
        Task ActualizarPermisosAsync(Permisos permisos);
        Task EliminarPermisosAsync(string id);
        Task<List<Permisos>> ObtenerTodosAsync();
    }
} 