using Microsoft.EntityFrameworkCore;
using Nexas.Inventory.Domain.Product.Interface;
using Nexas.Inventory.Domain.StockItem.Entity;
using Nexas.Inventory.Domain.Store.Entity;
using Nexas.Inventory.Infrastructure.Context;
using Nexas.Inventory.Infrastructure.StockItem.Interface;
using Nexas.Inventory.Infrastructure.Store.Interface;

namespace Nexas.Inventory.Infrastructure.StockItem.Repository;

public class StockItemRepository : BaseRepository<StockItemEntity>, IStockItemRepository
{
    private readonly IProductRepository _productRepository;

    public StockItemRepository(AppDbContext context,
        IProductRepository productRepository)
        : base(context)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<StockItemEntity>> getNegativeStock()
    {
        return await _dbSet.Where(x => x.Quantity < 0).ToListAsync();
    }

    public async Task<IEnumerable<StockItemEntity>> getPositiveStock()
    {
        return await _dbSet.Where(x => x.Quantity > 0).ToListAsync();
    }

    public async Task<IEnumerable<StockItemEntity>> getStockByProduct(int id)
    {
        return await _dbSet.Where(x => x.ProductId == id).ToListAsync();
    }

    public async Task<IEnumerable<StockItemEntity>> getStockByStore(int id)
    {
        return await _dbSet.Where(x => x.StoreId == id).ToListAsync();
    }
}

