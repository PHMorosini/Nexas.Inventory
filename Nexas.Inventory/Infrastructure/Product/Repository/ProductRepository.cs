using Microsoft.EntityFrameworkCore;
using Nexas.Inventory.Domain.Product.Entity;
using Nexas.Inventory.Domain.Product.Interface;

namespace Nexas.Inventory.Infrastructure.Product.Repository
{
    public class ProductRepository : BaseRepository<ProductEntity>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context) { }
        public async Task<IEnumerable<ProductEntity>> GetAllActiveProductAsync()
        {
            return await _dbSet.Where(p => p.Active).ToListAsync();
        }
    }
}
