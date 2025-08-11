using AutoMapper;
using Nexas.Inventory.Application.Base.Interface;
using Nexas.Inventory.Application.Base.Service;
using Nexas.Inventory.Application.Product.Interface;
using Nexas.Inventory.Application.Product.ViewModel;
using Nexas.Inventory.Domain.Product.Entity;
using Nexas.Inventory.Domain.Product.Interface;
using Nexas.Inventory.Infrastructure.Base.Interfaces;

namespace Nexas.Inventory.Application.Product.Services
{
    public class ProductService : BaseService<ProductEntity, ProductViewModel>, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository,IMapper mapper) : base(productRepository,mapper)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductViewModel>> getAllAtiveProductAsync()
        {
           var productsVm = await _productRepository.GetAllActiveProductAsync();
            return _mapper.Map<IEnumerable<ProductViewModel>>(productsVm);
        }
    }
}
