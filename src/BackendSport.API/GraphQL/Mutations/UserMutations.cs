using BackendSport.Application.UseCases.AuthUseCases;
using BackendSport.Application.DTOs.AuthDTOs;
using BackendSport.Domain.Entities.AuthEntities;
using HotChocolate.Authorization;

namespace BackendSport.API.GraphQL.Mutations;

/// <summary>
/// Mutaciones GraphQL para usuarios
/// </summary>
[ExtendObjectType("Mutation")]
public class UserMutations
{
    /// <summary>
    /// Crea un nuevo usuario
    /// </summary>
    /// <param name="input">Datos del usuario a crear</param>
    /// <param name="createUserUseCase">Caso de uso para crear usuarios</param>
    /// <returns>Usuario creado</returns>
    [Authorize]
    public async Task<UserResponseDto> CreateUserAsync(
        CreateUserDto input,
        [Service] CreateUserUseCase createUserUseCase)
    {
        return await createUserUseCase.ExecuteAsync(input);
    }

    /// <summary>
    /// Asigna un rol a un usuario
    /// </summary>
    /// <param name="input">Datos para asignar rol</param>
    /// <param name="assignRolUseCase">Caso de uso para asignar roles</param>
    /// <returns>Resultado de la operación</returns>
    [Authorize]
    public async Task<bool> AssignRoleToUserAsync(
        AsignarRolUsuarioDto input,
        [Service] AsignarRolAUsuarioUseCase assignRolUseCase)
    {
        await assignRolUseCase.ExecuteAsync(input);
        return true;
    }

    /// <summary>
    /// Asigna un deporte a un usuario
    /// </summary>
    /// <param name="input">Datos para asignar deporte</param>
    /// <param name="assignDeporteUseCase">Caso de uso para asignar deportes</param>
    /// <returns>Resultado de la operación</returns>
    [Authorize]
    public async Task<bool> AssignSportToUserAsync(
        AsignarDeporteUsuarioDto input,
        [Service] AsignarDeporteAUsuarioUseCase assignDeporteUseCase)
    {
        await assignDeporteUseCase.ExecuteAsync(input);
        return true;
    }

    /// <summary>
    /// Remueve un deporte de un usuario
    /// </summary>
    /// <param name="input">Datos para remover deporte</param>
    /// <param name="removeDeporteUseCase">Caso de uso para remover deportes</param>
    /// <returns>Resultado de la operación</returns>
    [Authorize]
    public async Task<bool> RemoveSportFromUserAsync(
        RemoverDeporteUsuarioDto input,
        [Service] RemoverDeporteDeUsuarioUseCase removeDeporteUseCase)
    {
        await removeDeporteUseCase.ExecuteAsync(input);
        return true;
    }
}
