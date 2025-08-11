using Nexas.Inventory.Application.Base.Interface;
using Nexas.Inventory.Application.Product.ViewModel;
using Nexas.Inventory.Application.StockItem.ViewModel;
using Nexas.Inventory.Domain.Product.Entity;

namespace Nexas.Inventory.Application.Product.Interface;
public interface IProductService : IBaseService<ProductEntity, ProductViewModel>
{
    Task<IEnumerable<ProductViewModel>> getAllAtiveProductAsync();

}
