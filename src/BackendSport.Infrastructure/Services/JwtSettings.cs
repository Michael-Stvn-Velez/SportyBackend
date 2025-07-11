namespace BackendSport.Infrastructure.Services;

/// <summary>
/// Configuración para JWT (JSON Web Tokens) con mejores prácticas de seguridad.
/// Esta clase contiene todos los parámetros necesarios para la generación y validación de tokens JWT.
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Clave secreta para firmar los tokens JWT. Debe tener al menos 32 caracteres para seguridad.
    /// </summary>
    /// <remarks>
    /// Esta clave debe mantenerse segura y no debe compartirse. En producción, debe almacenarse
    /// en variables de entorno o en un servicio de gestión de secretos.
    /// </remarks>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Emisor del token JWT. Identifica la aplicación que genera el token.
    /// </summary>
    /// <example>SportyBackend</example>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Audiencia del token JWT. Identifica los destinatarios para los que está destinado el token.
    /// </summary>
    /// <example>SportyBackendUsers</example>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Tiempo de expiración del access token en minutos.
    /// </summary>
    /// <remarks>
    /// Se recomienda un valor entre 15-30 minutos para access tokens.
    /// </remarks>
    public int AccessTokenExpirationMinutes { get; set; } = 15;

    /// <summary>
    /// Tiempo de expiración del refresh token en días.
    /// </summary>
    /// <remarks>
    /// Se recomienda un valor entre 7-30 días para refresh tokens.
    /// </remarks>
    public int RefreshTokenExpirationDays { get; set; } = 7;

    /// <summary>
    /// Indica si se requiere HTTPS para los metadatos de autenticación.
    /// </summary>
    public bool RequireHttpsMetadata { get; set; } = true;

    /// <summary>
    /// Indica si se debe validar la clave de firma del emisor.
    /// </summary>
    public bool ValidateIssuerSigningKey { get; set; } = true;

    /// <summary>
    /// Indica si se debe validar el emisor del token.
    /// </summary>
    public bool ValidateIssuer { get; set; } = true;

    /// <summary>
    /// Indica si se debe validar la audiencia del token.
    /// </summary>
    public bool ValidateAudience { get; set; } = true;

    /// <summary>
    /// Indica si se debe validar el tiempo de vida del token.
    /// </summary>
    public bool ValidateLifetime { get; set; } = true;

    /// <summary>
    /// Tolerancia de tiempo (en minutos) para la validación de expiración.
    /// </summary>
    /// <remarks>
    /// Permite una pequeña diferencia de tiempo entre el servidor y el cliente.
    /// </remarks>
    public int ClockSkewMinutes { get; set; } = 5;
} 