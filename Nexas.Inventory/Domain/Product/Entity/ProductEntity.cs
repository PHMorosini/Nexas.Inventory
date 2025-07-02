using Nexas.Inventory.Domain.StockItem.Entity;

namespace Nexas.Inventory.Domain.Product.Entity
{
    public class ProductEntity
    {
       

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public ICollection<StockItemEntity> StockItems { get; set; }

        public ProductEntity()
        {
        }
        public ProductEntity(int id, string name, decimal price, bool active)
        {
            Id = id;
            Name = name;
            Price = price;
            Active = active;
        }

        public ProductEntity(int id, string name, decimal price, ICollection<StockItemEntity> stockItems)
        {
            Id = id;
            Name = name;
            Price = price;
            StockItems = stockItems ?? new List<StockItemEntity>();
        }
        public void ActivateDesactivate() => Active = !Active;


    }
}
