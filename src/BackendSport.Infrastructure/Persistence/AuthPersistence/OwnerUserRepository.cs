using MongoDB.Driver;
using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Domain.Entities.AuthEntities;
using BackendSport.Domain.Services;

namespace BackendSport.Infrastructure.Persistence.AuthPersistence{
    public class OwnerUserRepository : IOwnerUserRepository
    {
        private readonly IMongoCollection<OwnerUser> _ownerUsers;

        public OwnerUserRepository(MongoDbContext context)
        {
            _ownerUsers = context.Database.GetCollection<OwnerUser>("ownerUsers");
        }
        public async Task<OwnerUser> CreateAsync(OwnerUser ownerUser)
        {
            await _ownerUsers.InsertOneAsync(ownerUser);
            return ownerUser;
        }
        public async Task<OwnerUser?> GetByEmailAsync(string email)
        {
            return await _ownerUsers.Find(u => u.Email == email).FirstOrDefaultAsync();
        }
        public async Task<OwnerUser?> GetByIdAsync(string id)
        {
            return await _ownerUsers.Find(u => u.Id == id).FirstOrDefaultAsync();
        }
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _ownerUsers.Find(u => u.Email == email).AnyAsync();
        }
        public async Task<bool> UpdateAsync(OwnerUser ownerUser)
        {
            ownerUser.UpdatedAt = DateTime.UtcNow;
            var result = await _ownerUsers.ReplaceOneAsync(u => u.Id == ownerUser.Id, ownerUser);
            return result.ModifiedCount > 0;
        }

    }
    
}