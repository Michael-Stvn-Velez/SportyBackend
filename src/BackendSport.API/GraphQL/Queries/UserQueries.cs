using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Domain.Entities.AuthEntities;
using HotChocolate.Authorization;

namespace BackendSport.API.GraphQL.Queries;

/// <summary>
/// Consultas GraphQL para usuarios
/// </summary>
[ExtendObjectType("Query")]
public class UserQueries
{
    /// <summary>
    /// Obtiene un usuario por su ID
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="userRepository">Repositorio de usuarios</param>
    /// <returns>Usuario encontrado o null</returns>
    [Authorize]
    public async Task<User?> GetUserByIdAsync(
        string id, 
        [Service] IUserRepository userRepository)
    {
        return await userRepository.GetByIdAsync(id);
    }

    /// <summary>
    /// Obtiene un usuario por su email
    /// </summary>
    /// <param name="email">Email del usuario</param>
    /// <param name="userRepository">Repositorio de usuarios</param>
    /// <returns>Usuario encontrado o null</returns>
    [Authorize]
    public async Task<User?> GetUserByEmailAsync(
        string email, 
        [Service] IUserRepository userRepository)
    {
        return await userRepository.GetByEmailAsync(email);
    }
}
