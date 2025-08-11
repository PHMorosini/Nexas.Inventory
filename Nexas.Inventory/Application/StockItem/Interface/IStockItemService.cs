using Nexas.Inventory.Application.Base.Interface;
using Nexas.Inventory.Application.Product.ViewModel;
using Nexas.Inventory.Application.StockItem.ViewModel;
using Nexas.Inventory.Domain.Product.Entity;
using Nexas.Inventory.Domain.StockItem.Entity;

namespace Nexas.Inventory.Application.StockItem.Interface;
public interface IStockItemService : IBaseService<StockItemEntity, StockItemViewModel>
{
    Task<IEnumerable<StockItemViewModel>> getNegativeStock();
    Task<IEnumerable<StockItemViewModel>> getPositiveStock();
    Task<IEnumerable<StockItemViewModel>> getStockByProduct(int id);
    Task<IEnumerable<StockItemViewModel>> getStockByStore(int id);
}

