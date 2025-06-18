using Nexas.Inventory.Domain.Product.Entity;
using Nexas.Inventory.Infrastructure.Base.Interfaces;

namespace Nexas.Inventory.Domain.Product.Interface
{
    public interface IProductRepository : IBaseRepository<ProductEntity>
    {
        Task<IEnumerable<ProductEntity>> GetAllActiveProductAsync();
    }
}
