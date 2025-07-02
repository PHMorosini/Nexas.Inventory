using Nexas.Inventory.Domain.Product.Entity;
using Nexas.Inventory.Domain.Store.Entity;
using Nexas.Inventory.Infrastructure.Base.Interfaces;

namespace Nexas.Inventory.Infrastructure.Store.Interface
{
    public interface IStoreRepository : IBaseRepository<StoreEntity>
    {
    }
}
