using Microsoft.EntityFrameworkCore;
using Nexas.Inventory.Domain.Product.Entity;
using Nexas.Inventory.Domain.StockItem.Entity;
using Nexas.Inventory.Domain.Store.Entity;
using Nexas.Inventory.Domain.User.Entity;

namespace Nexas.Inventory.Infrastructure.Context;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }



    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<StoreEntity> Stores { get; set; }
    public DbSet<StockItemEntity> StockItem { get; set; }
    public DbSet<UserEntity> UserEntities { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<UserEntity>()
            .Property(u => u.Name)
            .IsRequired();

    }
}
