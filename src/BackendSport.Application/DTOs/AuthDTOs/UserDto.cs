using System.ComponentModel.DataAnnotations;

namespace BackendSport.Application.DTOs.AuthDTOs;

public class CreateUserDto
{
    [Required(ErrorMessage = "El nombre es requerido")]
    [MinLength(2, ErrorMessage = "El nombre debe tener al menos 2 caracteres")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El email es requerido")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
    public string Password { get; set; } = string.Empty;
    
    // Campos de documento
    [Required(ErrorMessage = "El tipo de documento es requerido")]
    public string DocumentTypeId { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El número de documento es requerido")]
    public string DocumentNumber { get; set; } = string.Empty;
    
    // Campos de ubicación
    [Required(ErrorMessage = "El país es requerido")]
    public string CountryId { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El departamento es requerido")]
    public string DepartmentId { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El municipio es requerido")]
    public string MunicipalityId { get; set; } = string.Empty;
    
    public string? LocalityId { get; set; } = null;
}

public class UserResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DocumentTypeId { get; set; } = string.Empty;
    public string DocumentNumber { get; set; } = string.Empty;
    public string CountryId { get; set; } = string.Empty;
    public string? DepartmentId { get; set; } = null;
    public string? MunicipalityId { get; set; } = null;
    public string? LocalityId { get; set; } = null;
    public DateTime CreatedAt { get; set; }
}

public class AsignarRolUsuarioDto
{
    public string UserId { get; set; } = string.Empty;
    public string RolId { get; set; } = string.Empty;
}

public class AsignarDeporteUsuarioDto
{
    [Required(ErrorMessage = "El ID del usuario es requerido")]
    public string UserId { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El ID del deporte es requerido")]
    public string SportId { get; set; } = string.Empty;
}

public class RemoverDeporteUsuarioDto
{
    [Required(ErrorMessage = "El ID del usuario es requerido")]
    public string UserId { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "El ID del deporte es requerido")]
    public string SportId { get; set; } = string.Empty;
} 