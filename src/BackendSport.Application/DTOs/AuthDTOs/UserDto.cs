using System.ComponentModel.DataAnnotations;

namespace BackendSport.Application.DTOs.AuthDTOs;

public class CreateUserDto
{
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string Password { get; set; } = string.Empty;
}

public class UserResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class AsignarRolUsuarioDto
{
    public string UserId { get; set; } = string.Empty;
    public string RolId { get; set; } = string.Empty;
} 