using BackendSport.Application.Interfaces.RolInterfaces;
using BackendSport.Application.Interfaces.PermisosInterfaces;
using BackendSport.Domain.Entities.RolEntities;
using BackendSport.Domain.Entities.PermisosEntities;
using HotChocolate.Authorization;

namespace BackendSport.API.GraphQL.Queries;

/// <summary>
/// Consultas GraphQL para roles y permisos
/// </summary>
[ExtendObjectType("Query")]
public class SecurityQueries
{
    /// <summary>
    /// Obtiene todos los roles del sistema
    /// </summary>
    /// <param name="rolRepository">Repositorio de roles</param>
    /// <returns>Lista de roles</returns>
    [Authorize]
    public async Task<IEnumerable<Rol>> GetRolesAsync([Service] IRolRepository rolRepository)
    {
        return await rolRepository.ObtenerTodosAsync();
    }

    /// <summary>
    /// Obtiene un rol por su ID
    /// </summary>
    /// <param name="id">ID del rol</param>
    /// <param name="rolRepository">Repositorio de roles</param>
    /// <returns>Rol encontrado o null</returns>
    [Authorize]
    public async Task<Rol?> GetRolByIdAsync(
        string id, 
        [Service] IRolRepository rolRepository)
    {
        return await rolRepository.ObtenerPorIdAsync(id);
    }

    /// <summary>
    /// Obtiene todos los permisos del sistema
    /// </summary>
    /// <param name="permisosRepository">Repositorio de permisos</param>
    /// <returns>Lista de permisos</returns>
    [Authorize]
    public async Task<IEnumerable<Permisos>> GetPermissionsAsync([Service] IPermisosRepository permisosRepository)
    {
        return await permisosRepository.ObtenerTodosAsync();
    }

    /// <summary>
    /// Obtiene un permiso por su ID
    /// </summary>
    /// <param name="id">ID del permiso</param>
    /// <param name="permisosRepository">Repositorio de permisos</param>
    /// <returns>Permiso encontrado o null</returns>
    [Authorize]
    public async Task<Permisos?> GetPermissionByIdAsync(
        string id, 
        [Service] IPermisosRepository permisosRepository)
    {
        return await permisosRepository.ObtenerPorIdAsync(id);
    }

    /// <summary>
    /// Busca roles por nombre
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda</param>
    /// <param name="rolRepository">Repositorio de roles</param>
    /// <returns>Lista de roles que coinciden con la búsqueda</returns>
    [Authorize]
    public async Task<IEnumerable<Rol>> SearchRolesAsync(
        string searchTerm,
        [Service] IRolRepository rolRepository)
    {
        var allRoles = await rolRepository.ObtenerTodosAsync();
        return allRoles.Where(r => 
            r.Nombre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            r.Descripcion.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Busca permisos por nombre
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda</param>
    /// <param name="permisosRepository">Repositorio de permisos</param>
    /// <returns>Lista de permisos que coinciden con la búsqueda</returns>
    [Authorize]
    public async Task<IEnumerable<Permisos>> SearchPermissionsAsync(
        string searchTerm,
        [Service] IPermisosRepository permisosRepository)
    {
        var allPermissions = await permisosRepository.ObtenerTodosAsync();
        return allPermissions.Where(p => 
            p.Nombre.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            p.Descripcion.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
    }
}
