using Nexas.Inventory.Domain.StockItem.Entity;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Xml.Linq;

namespace Nexas.Inventory.Domain.Store.Entity
{
    public class StoreEntity
    {
            [Key]
            public int Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }

            public ICollection<StockItemEntity> StockItems { get; set; }

            public StoreEntity(int id, string name, string address, ICollection<StockItemEntity> stockItems)
            {
                Id = id;
                Name = name;
                Address = address;
                StockItems = stockItems;
            }

            public StoreEntity()
            {
            }

    }
}
