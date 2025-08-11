using Nexas.Inventory.Application.Base.Interface;
using Nexas.Inventory.Application.StockItem.ViewModel;
using Nexas.Inventory.Application.Store.ViewModel;
using Nexas.Inventory.Domain.StockItem.Entity;
using Nexas.Inventory.Domain.Store.Entity;

namespace Nexas.Inventory.Application.Store.Interface;

public interface IStoreService : IBaseService<StoreEntity, StoreViewModel>
{
}

