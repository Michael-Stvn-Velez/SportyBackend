using MongoDB.Driver;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Domain.Entities.AuthEntities;

namespace BackendSport.Infrastructure.Persistence.AuthPersistence{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(MongoDbContext context)
        {
            _users = context.Database.GetCollection<User>("users");
        }
        public async Task<User> CreateAsync(User user)
        {
            await _users.InsertOneAsync(user);
            return user;
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        }
        public async Task<User?> GetByIdAsync(string id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email).AnyAsync();
        }
        public async Task<bool> AddRolToUserAsync(string userId, string rolId)
        {
            var update = Builders<User>.Update.AddToSet(u => u.RolIds, rolId);
            var result = await _users.UpdateOneAsync(u => u.Id == userId, update);
            return result.ModifiedCount > 0;
        }
        public async Task<bool> UpdateAsync(User user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            var result = await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
            return result.ModifiedCount > 0;
        }
        public async Task<bool> AddSportToUserAsync(string userId, string sportId)
        {
            var user = await GetByIdAsync(userId);
            if (user == null) return false;

            if (user.AddSport(sportId))
            {
                return await UpdateAsync(user);
            }
            return false;
        }
        public async Task<bool> RemoveSportFromUserAsync(string userId, string sportId)
        {
            var user = await GetByIdAsync(userId);
            if (user == null) return false;

            var sportToRemove = user.Sports.FirstOrDefault(s => s.SportId == sportId);
            if (sportToRemove != null)
            {
                user.Sports.Remove(sportToRemove);
                user.UpdatedAt = DateTime.UtcNow;
                return await UpdateAsync(user);
            }
            return false;
        }
    } 
}