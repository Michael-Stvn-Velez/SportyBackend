using BackendSport.Domain.Entities.AuthEntities;

namespace BackendSport.Application.Interfaces.AuthInterfaces{
    public interface IUserRepository
    {
       Task<User> CreateAsync(User user);
       Task<User?> GetByEmailAsync(string email);
       Task<User?> GetByIdAsync(string id);
       Task<bool> ExistsByEmailAsync(string email);
       Task<bool> AddRolToUserAsync(string userId, string rolId);
    } 

}