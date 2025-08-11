using AutoMapper;
using Nexas.Inventory.Application.Product.ViewModel;
using Nexas.Inventory.Domain.Product.Entity;

namespace Nexas.Inventory.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<ProductViewModel, ProductEntity>();
            CreateMap<ProductEntity, ProductViewModel>();
        }
    }
}
