using Microsoft.EntityFrameworkCore;
using Nexas.Inventory.Domain.Product.Entity;
using Nexas.Inventory.Domain.StockItem.Entity;
using Nexas.Inventory.Domain.Store.Entity;
using Nexas.Inventory.Domain.User.Entity;

namespace Nexas.Inventory.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected AppDbContext()
        {
        }

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<StoreEntity> Stores { get; set; }
        public DbSet<StockItemEntity> StockItem { get; set; }
    }
}
