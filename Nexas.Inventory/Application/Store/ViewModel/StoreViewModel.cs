using Nexas.Inventory.Application.StockItem.ViewModel;

namespace Nexas.Inventory.Application.Store.ViewModel
{
    public class StoreViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public List<StockItemViewModel> StockItems { get; set; }
    }
}
