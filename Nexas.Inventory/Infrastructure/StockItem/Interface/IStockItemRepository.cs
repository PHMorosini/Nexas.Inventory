using Nexas.Inventory.Domain.Product.Entity;
using Nexas.Inventory.Domain.StockItem.Entity;
using Nexas.Inventory.Domain.Store.Entity;
using Nexas.Inventory.Infrastructure.Base.Interfaces;

namespace Nexas.Inventory.Infrastructure.StockItem.Interface;
public interface IStockItemRepository : IBaseRepository<StockItemEntity>
{
    Task<IEnumerable<StockItemEntity>> getStockByStore(int id);
    Task<IEnumerable<StockItemEntity>> getStockByProduct(int id);
    Task<IEnumerable<StockItemEntity>> getPositiveStock();
    Task<IEnumerable<StockItemEntity>> getNegativeStock();
}

