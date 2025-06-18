using Nexas.Inventory.Domain.Product.Entity;
using Nexas.Inventory.Domain.Store.Entity;
using System.ComponentModel.DataAnnotations;

namespace Nexas.Inventory.Domain.StockItem.Entity
{
    public class StockItemEntity
    {
        [Key]
        public Guid StockItemId { get; set; } = Guid.NewGuid();

        public int ProductId { get; set; }
        public virtual ProductEntity Product { get; set; }


        public int StoreId { get; set; }
        public virtual StoreEntity Store { get; set; }

        public decimal Quantity { get; set; }
        public StockItemEntity()
        {
        }

        public StockItemEntity(int produtoId, int storeId, decimal quantity)
        {
            ProductId = produtoId;
            StoreId = storeId;
            Quantity = quantity;
        }
        public StockItemEntity(Guid stockItemId, int produtoId, int storeId, decimal quantity)
        {
            StockItemId = stockItemId;
            ProductId = produtoId;
            StoreId = storeId;
            Quantity = quantity;
        }
    }
}
