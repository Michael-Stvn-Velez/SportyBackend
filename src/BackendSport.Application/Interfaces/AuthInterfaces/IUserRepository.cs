using BackendSport.Domain.Entities.AuthEntities;

namespace BackendSport.Application.Interfaces.AuthInterfaces;

/// <summary>
/// Interfaz para el repositorio de usuarios
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Crea un nuevo usuario
    /// </summary>
    /// <param name="user">Usuario a crear</param>
    /// <returns>Usuario creado</returns>
    Task<User> CreateAsync(User user);
    
    /// <summary>
    /// Busca un usuario por email
    /// </summary>
    /// <param name="email">Email del usuario</param>
    /// <returns>Usuario encontrado o null</returns>
    Task<User?> GetByEmailAsync(string email);
    
    /// <summary>
    /// Verifica si existe un usuario con el email especificado
    /// </summary>
    /// <param name="email">Email a verificar</param>
    /// <returns>True si existe, false en caso contrario</returns>
    Task<bool> ExistsByEmailAsync(string email);
} 