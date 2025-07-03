using System.ComponentModel.DataAnnotations;

namespace BackendSport.Application.DTOs.AuthDTOs;

/// <summary>
/// DTO para crear un nuevo usuario
/// </summary>
public class CreateUserDto
{
    /// <summary>
    /// Email del usuario (debe ser único)
    /// </summary>
    /// <example>usuario@ejemplo.com</example>
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Contraseña del usuario (mínimo 6 caracteres)
    /// </summary>
    /// <example>123456</example>
    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// DTO para la respuesta de usuario creado
/// </summary>
public class UserResponseDto
{
    /// <summary>
    /// ID único del usuario
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// Email del usuario
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Fecha de creación del usuario
    /// </summary>
    public DateTime CreatedAt { get; set; }
} 