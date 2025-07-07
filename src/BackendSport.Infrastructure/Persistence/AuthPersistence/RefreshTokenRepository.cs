using BackendSport.Application.Interfaces.AuthInterfaces;
using BackendSport.Domain.Entities.AuthEntities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendSport.Infrastructure.Persistence.AuthPersistence
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IMongoCollection<RefreshToken> _collection;

        public RefreshTokenRepository(MongoDbContext context)
        {
            _collection = context.Database.GetCollection<RefreshToken>("RefreshTokens");
        }

        public async Task CreateAsync(RefreshToken token)
        {
            await _collection.InsertOneAsync(token);
        }

        public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
        {
            return await _collection.Find(x => x.TokenHash == tokenHash && !x.Revoked).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RefreshToken>> GetByUserIdAsync(string userId)
        {
            return await _collection.Find(x => x.UserId == userId && !x.Revoked).ToListAsync();
        }

        public async Task RevokeAsync(string tokenId)
        {
            var update = Builders<RefreshToken>.Update.Set(x => x.Revoked, true);
            await _collection.UpdateOneAsync(x => x.Id == tokenId, update);
        }

        public async Task DeleteAsync(string tokenId)
        {
            await _collection.DeleteOneAsync(x => x.Id == tokenId);
        }

        public async Task<IEnumerable<RefreshToken>> GetAllActiveAsync()
        {
            return await _collection.Find(x => !x.Revoked && x.ExpiresAt > DateTime.UtcNow).ToListAsync();
        }

        public async Task<RefreshToken?> GetByTokenIdAsync(string tokenId)
        {
            return await _collection.Find(x => x.TokenId == tokenId && !x.Revoked).FirstOrDefaultAsync();
        }

        public async Task RevokeAllByUserIdAsync(string userId)
        {
            var filter = Builders<RefreshToken>.Filter.Where(x => x.UserId == userId && !x.Revoked && x.ExpiresAt > DateTime.UtcNow);
            var update = Builders<RefreshToken>.Update.Set(x => x.Revoked, true);
            await _collection.UpdateManyAsync(filter, update);
        }
    }
} 