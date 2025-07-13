using BackendSport.Domain.Entities.AuthEntities;
using BackendSport.Domain.Entities.DeporteEntities;
using HotChocolate.Authorization;
using HotChocolate.Subscriptions;

namespace BackendSport.API.GraphQL.Subscriptions;

/// <summary>
/// Suscripciones GraphQL para eventos del sistema
/// </summary>
[ExtendObjectType("Subscription")]
public class SystemSubscriptions
{
    /// <summary>
    /// Suscripción para cambios en usuarios
    /// </summary>
    /// <param name="user">Usuario que ha cambiado</param>
    /// <returns>Stream de eventos de usuario</returns>
    [Authorize]
    [Subscribe]
    [Topic("user_changes")]
    public User OnUserChanged([EventMessage] User user) => user;

    /// <summary>
    /// Suscripción para nuevos usuarios creados
    /// </summary>
    /// <param name="user">Usuario que ha sido creado</param>
    /// <returns>Stream de nuevos usuarios</returns>
    [Authorize]
    [Subscribe]
    [Topic("user_created")]
    public User OnUserCreated([EventMessage] User user) => user;

    /// <summary>
    /// Suscripción para cambios en deportes
    /// </summary>
    /// <param name="deporte">Deporte que ha cambiado</param>
    /// <returns>Stream de eventos de deporte</returns>
    [Authorize]
    [Subscribe]
    [Topic("deporte_changes")]
    public Deporte OnDeporteChanged([EventMessage] Deporte deporte) => deporte;

    /// <summary>
    /// Suscripción para nuevos deportes creados
    /// </summary>
    /// <param name="deporte">Deporte que ha sido creado</param>
    /// <returns>Stream de nuevos deportes</returns>
    [Authorize]
    [Subscribe]
    [Topic("deporte_created")]
    public Deporte OnDeporteCreated([EventMessage] Deporte deporte) => deporte;

    /// <summary>
    /// Suscripción para notificaciones del sistema
    /// </summary>
    /// <param name="notification">Notificación del sistema</param>
    /// <returns>Stream de notificaciones</returns>
    [Authorize]
    [Subscribe]
    [Topic("system_notifications")]
    public SystemNotification OnSystemNotification([EventMessage] SystemNotification notification) => notification;
}

/// <summary>
/// Modelo para notificaciones del sistema
/// </summary>
public class SystemNotification
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string UserId { get; set; } = string.Empty;
    public Dictionary<string, object> Metadata { get; set; } = new();
}
