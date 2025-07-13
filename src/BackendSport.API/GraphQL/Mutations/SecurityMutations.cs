using BackendSport.Application.UseCases.RolUseCases;
using BackendSport.Application.UseCases.PermisosUseCases;
using BackendSport.Application.DTOs.RolDTOs;
using BackendSport.Application.DTOs.PermisosDTOs;
using HotChocolate.Authorization;

namespace BackendSport.API.GraphQL.Mutations;

/// <summary>
/// Mutaciones GraphQL para roles y permisos
/// </summary>
[ExtendObjectType("Mutation")]
public class SecurityMutations
{
    /// <summary>
    /// Crea un nuevo rol
    /// </summary>
    /// <param name="input">Datos del rol a crear</param>
    /// <param name="createRolUseCase">Caso de uso para crear roles</param>
    /// <returns>Rol creado</returns>
    [Authorize]
    public async Task<RolListDto> CreateRoleAsync(
        RolDto input,
        [Service] CreateRolUseCase createRolUseCase)
    {
        return await createRolUseCase.ExecuteAsync(input);
    }

    /// <summary>
    /// Actualiza un rol existente
    /// </summary>
    /// <param name="id">ID del rol a actualizar</param>
    /// <param name="input">Nuevos datos del rol</param>
    /// <param name="updateRolUseCase">Caso de uso para actualizar roles</param>
    /// <returns>Resultado de la operación</returns>
    [Authorize]
    public async Task<bool> UpdateRoleAsync(
        string id,
        RolUpdateDto input,
        [Service] UpdateRolUseCase updateRolUseCase)
    {
        await updateRolUseCase.ExecuteAsync(id, input);
        return true;
    }

    /// <summary>
    /// Elimina un rol
    /// </summary>
    /// <param name="id">ID del rol a eliminar</param>
    /// <param name="deleteRolUseCase">Caso de uso para eliminar roles</param>
    /// <returns>Resultado de la operación</returns>
    [Authorize]
    public async Task<bool> DeleteRoleAsync(
        string id,
        [Service] DeleteRolUseCase deleteRolUseCase)
    {
        await deleteRolUseCase.ExecuteAsync(id);
        return true;
    }

    /// <summary>
    /// Crea un nuevo permiso
    /// </summary>
    /// <param name="input">Datos del permiso a crear</param>
    /// <param name="createPermisosUseCase">Caso de uso para crear permisos</param>
    /// <returns>Permiso creado</returns>
    [Authorize]
    public async Task<PermisosListDto> CreatePermissionAsync(
        PermisosDto input,
        [Service] CreatePermisosUseCase createPermisosUseCase)
    {
        return await createPermisosUseCase.ExecuteAsync(input);
    }

    /// <summary>
    /// Actualiza un permiso existente
    /// </summary>
    /// <param name="id">ID del permiso a actualizar</param>
    /// <param name="input">Nuevos datos del permiso</param>
    /// <param name="updatePermisosUseCase">Caso de uso para actualizar permisos</param>
    /// <returns>Resultado de la operación</returns>
    [Authorize]
    public async Task<bool> UpdatePermissionAsync(
        string id,
        PermisosUpdateDto input,
        [Service] UpdatePermisosUseCase updatePermisosUseCase)
    {
        await updatePermisosUseCase.ExecuteAsync(id, input);
        return true;
    }

    /// <summary>
    /// Elimina un permiso
    /// </summary>
    /// <param name="id">ID del permiso a eliminar</param>
    /// <param name="deletePermisosUseCase">Caso de uso para eliminar permisos</param>
    /// <returns>Resultado de la operación</returns>
    [Authorize]
    public async Task<bool> DeletePermissionAsync(
        string id,
        [Service] DeletePermisosUseCase deletePermisosUseCase)
    {
        await deletePermisosUseCase.ExecuteAsync(id);
        return true;
    }
}
