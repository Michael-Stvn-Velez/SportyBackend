using BackendSport.Application.Services;
using BCrypt.Net;

namespace BackendSport.Infrastructure.Services;

/// <summary>
/// Implementación del servicio de hash de contraseñas usando BCrypt
/// </summary>
public class PasswordService : IPasswordService
{
    /// <summary>
    /// Genera un hash de la contraseña usando BCrypt
    /// </summary>
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
    }

    /// <summary>
    /// Verifica si una contraseña coincide con su hash
    /// </summary>
    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
} 