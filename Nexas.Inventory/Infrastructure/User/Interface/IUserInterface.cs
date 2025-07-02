using Nexas.Inventory.Domain.User.Entity;
using Nexas.Inventory.Infrastructure.Base.Interfaces;

namespace Nexas.Inventory.Infrastructure.User.Interface
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        Task<UserEntity> GetByEmailAsync(string email);
    }

}
