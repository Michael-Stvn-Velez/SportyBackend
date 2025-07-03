namespace BackendSport.Application.Services;

/// <summary>
/// Interfaz para el servicio de hash de contraseñas
/// </summary>
public interface IPasswordService
{
    /// <summary>
    /// Genera un hash de la contraseña
    /// </summary>
    /// <param name="password">Contraseña en texto plano</param>
    /// <returns>Hash de la contraseña</returns>
    string HashPassword(string password);
    
    /// <summary>
    /// Verifica si una contraseña coincide con su hash
    /// </summary>
    /// <param name="password">Contraseña en texto plano</param>
    /// <param name="hashedPassword">Hash de la contraseña</param>
    /// <returns>True si coinciden, false en caso contrario</returns>
    bool VerifyPassword(string password, string hashedPassword);
} 