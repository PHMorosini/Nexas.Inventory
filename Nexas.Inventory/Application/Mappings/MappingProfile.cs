using AutoMapper;
using Nexas.Inventory.Application.Product.ViewModel;
using Nexas.Inventory.Application.StockItem.ViewModel;
using Nexas.Inventory.Domain.Product.Entity;
using Nexas.Inventory.Domain.StockItem.Entity;

namespace Nexas.Inventory.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            //PRODUCT MAPPER
            CreateMap<ProductViewModel, ProductEntity>();
            CreateMap<ProductEntity, ProductViewModel>();

            //STOCKITEM MAPPER
            CreateMap<StockItemViewModel, StockItemEntity>();
            CreateMap<StockItemEntity, StockItemViewModel>();
        }
    }
}
