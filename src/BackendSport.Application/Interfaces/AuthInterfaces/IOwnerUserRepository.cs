using BackendSport.Domain.Entities.AuthEntities;

namespace BackendSport.Application.Interfaces.AuthInterfaces{
    public interface IOwnerUserRepository
    {
       Task<OwnerUser> CreateAsync(OwnerUser ownerUser);
       Task<OwnerUser?> GetByEmailAsync(string email);
       Task<OwnerUser?> GetByIdAsync(string id);
       Task<bool> ExistsByEmailAsync(string email);
       Task<bool> UpdateAsync(OwnerUser ownerUser);
    } 

}