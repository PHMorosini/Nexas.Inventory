using Microsoft.EntityFrameworkCore;
using Nexas.Inventory.Domain.User.Entity;
using Nexas.Inventory.Infrastructure.Context;
using Nexas.Inventory.Infrastructure.User.Interface;

namespace Nexas.Inventory.Infrastructure.User.Repository
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<UserEntity> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ValidateUserPassword(UserEntity user)
        {
            return user.VerifyPassword(user.PasswordHash);
        }
    }

}
