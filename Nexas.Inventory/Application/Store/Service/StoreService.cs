using AutoMapper;
using Nexas.Inventory.Application.Base.Service;
using Nexas.Inventory.Application.Product.Interface;
using Nexas.Inventory.Application.Product.ViewModel;
using Nexas.Inventory.Application.Store.Interface;
using Nexas.Inventory.Application.Store.ViewModel;
using Nexas.Inventory.Domain.Product.Entity;
using Nexas.Inventory.Domain.Store.Entity;
using Nexas.Inventory.Infrastructure.Store.Interface;

namespace Nexas.Inventory.Application.Store.Service;

public class StoreService : BaseService<StoreEntity, StoreViewModel>, IStoreService
{
    private readonly IStoreRepository _storeRepository;
    
    public StoreService(IStoreRepository storeRepository,IMapper mapper) : base(storeRepository, mapper)
    {
        _storeRepository = storeRepository;
    }
}

