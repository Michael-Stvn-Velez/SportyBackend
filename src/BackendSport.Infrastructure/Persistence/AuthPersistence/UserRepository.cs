using MongoDB.Driver;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Domain.Entities.AuthEntities;

namespace BackendSport.Infrastructure.Persistence.AuthPersistence;

/// <summary>
/// Implementación del repositorio de usuarios para MongoDB
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _users;

    public UserRepository(MongoDbContext context)
    {
        _users = context.Database.GetCollection<User>("users");
    }

    /// <summary>
    /// Crea un nuevo usuario
    /// </summary>
    public async Task<User> CreateAsync(User user)
    {
        await _users.InsertOneAsync(user);
        return user;
    }

    /// <summary>
    /// Busca un usuario por email
    /// </summary>
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
    }

    /// <summary>
    /// Verifica si existe un usuario con el email especificado
    /// </summary>
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _users.Find(u => u.Email == email).AnyAsync();
    }
} 