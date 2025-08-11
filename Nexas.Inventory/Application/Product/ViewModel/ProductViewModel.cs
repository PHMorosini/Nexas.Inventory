using Nexas.Inventory.Application.StockItem.ViewModel;
using Nexas.Inventory.Domain.StockItem.Entity;
using System.ComponentModel.DataAnnotations;

namespace Nexas.Inventory.Application.Product.ViewModel
{
    public class ProductViewModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "The Product Name is required.")]
        [StringLength(120,MinimumLength = 3,ErrorMessage = "Product Name must be between 3 and 120 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The Product Price is required.")]
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public ICollection<StockItemViewModel> StockItems { get; set; }
    }
}
