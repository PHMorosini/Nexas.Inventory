﻿using Microsoft.EntityFrameworkCore;
using Nexas.Inventory.Domain.User.Entity;
using Nexas.Inventory.Infrastructure.User.Interface;

namespace Nexas.Inventory.Infrastructure.User.Repository
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(DbContext context) : base(context)
        {
        }

        public async Task<UserEntity> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }

}
