using BackendSport.Domain.Entities.RolEntities;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BackendSport.Application.Interfaces.RolInterfaces
{
    public interface IRolRepository
    {
        Task<Rol> CrearRolAsync(Rol rol);
        Task<bool> ExisteNombreAsync(string nombre);
        Task<Rol> ObtenerPorIdAsync(string id);
        Task ActualizarRolAsync(Rol rol);
        Task EliminarRolAsync(string id);
        Task<List<Rol>> ObtenerTodosAsync();
    }
} 