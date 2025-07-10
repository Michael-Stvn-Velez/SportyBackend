using BackendSport.Domain.Entities.AuthEntities;

namespace BackendSport.Domain.Services;

public class UserSportService
{
    private const int MAX_SPORTS_PER_USER = 3;

    /// <summary>
    /// Agrega un deporte al usuario si cumple con las validaciones de negocio
    /// </summary>
    /// <param name="user">Usuario al que se le agregará el deporte</param>
    /// <param name="sportId">ID del deporte a agregar</param>
    /// <returns>True si se agregó exitosamente, False en caso contrario</returns>
    public static bool AddSportToUser(User user, string sportId)
    {
        if (user == null)
            return false;

        if (user.Sports.Count >= MAX_SPORTS_PER_USER)
            return false;

        if (user.Sports.Any(s => s.SportId == sportId))
            return false;

        user.Sports.Add(new UserSport { SportId = sportId });
        user.UpdatedAt = DateTime.UtcNow;
        return true;
    }

    /// <summary>
    /// Configura un deporte específico del usuario con posiciones, nivel y métricas de rendimiento
    /// </summary>
    /// <param name="user">Usuario que contiene el deporte</param>
    /// <param name="sportId">ID del deporte a configurar</param>
    /// <param name="positions">Lista de posiciones del usuario</param>
    /// <param name="level">Nivel competitivo del usuario</param>
    /// <param name="performanceMetrics">Métricas de rendimiento del usuario</param>
    /// <returns>True si se configuró exitosamente, False en caso contrario</returns>
    public static bool ConfigureUserSport(User user, string sportId, List<string> positions, string level, Dictionary<string, int> performanceMetrics)
    {
        if (user == null)
            return false;

        var sport = user.Sports.FirstOrDefault(s => s.SportId == sportId);
        if (sport == null)
            return false;

        sport.Positions = positions;
        sport.Level = level;
        sport.PerformanceMetrics = performanceMetrics;
        sport.ConfiguredAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        return true;
    }

    /// <summary>
    /// Verifica si el usuario tiene un deporte específico asignado
    /// </summary>
    /// <param name="user">Usuario a verificar</param>
    /// <param name="sportId">ID del deporte a verificar</param>
    /// <returns>True si el usuario tiene el deporte, False en caso contrario</returns>
    public static bool UserHasSport(User user, string sportId)
    {
        if (user == null)
            return false;

        return user.Sports.Any(s => s.SportId == sportId);
    }

    /// <summary>
    /// Verifica si un deporte específico del usuario está configurado
    /// </summary>
    /// <param name="user">Usuario a verificar</param>
    /// <param name="sportId">ID del deporte a verificar</param>
    /// <returns>True si el deporte está configurado, False en caso contrario</returns>
    public static bool IsUserSportConfigured(User user, string sportId)
    {
        if (user == null)
            return false;

        var sport = user.Sports.FirstOrDefault(s => s.SportId == sportId);
        return sport?.ConfiguredAt != null;
    }

    /// <summary>
    /// Obtiene la cantidad de deportes que tiene el usuario
    /// </summary>
    /// <param name="user">Usuario a verificar</param>
    /// <returns>Número de deportes del usuario</returns>
    public static int GetUserSportsCount(User user)
    {
        if (user == null)
            return 0;

        return user.Sports.Count;
    }

    /// <summary>
    /// Verifica si el usuario puede agregar más deportes
    /// </summary>
    /// <param name="user">Usuario a verificar</param>
    /// <returns>True si puede agregar más deportes, False si ya tiene el máximo</returns>
    public static bool CanUserAddMoreSports(User user)
    {
        if (user == null)
            return false;

        return user.Sports.Count < MAX_SPORTS_PER_USER;
    }

    /// <summary>
    /// Obtiene el deporte específico del usuario
    /// </summary>
    /// <param name="user">Usuario que contiene el deporte</param>
    /// <param name="sportId">ID del deporte a obtener</param>
    /// <returns>El deporte del usuario o null si no existe</returns>
    public static UserSport? GetUserSport(User user, string sportId)
    {
        if (user == null)
            return null;

        return user.Sports.FirstOrDefault(s => s.SportId == sportId);
    }
} 