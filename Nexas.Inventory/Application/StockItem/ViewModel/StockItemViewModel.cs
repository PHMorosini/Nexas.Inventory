namespace Nexas.Inventory.Application.StockItem.ViewModel
{
    public class StockItemViewModel
    {
        public Guid StockItemId { get; set; }
        public int ProductId { get; set; }
        public int StoreId { get; set; }
        public decimal Quantity { get; set; }
    }
}
