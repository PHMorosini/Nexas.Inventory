using Microsoft.EntityFrameworkCore;
using Nexas.Inventory.Domain.Product.Entity;
using Nexas.Inventory.Domain.Product.Interface;
using Nexas.Inventory.Domain.Store.Entity;
using Nexas.Inventory.Infrastructure.Context;
using Nexas.Inventory.Infrastructure.Store.Interface;

namespace Nexas.Inventory.Infrastructure.Store.Repository
{
    public class StoreRepository : BaseRepository<StoreEntity>, IStoreRepository
    {
        public StoreRepository(AppDbContext context) : base(context) { }
    }
}
