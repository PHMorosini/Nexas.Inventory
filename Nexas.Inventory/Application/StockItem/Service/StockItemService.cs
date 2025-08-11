using AutoMapper;
using Nexas.Inventory.Application.Base.Service;
using Nexas.Inventory.Application.StockItem.Interface;
using Nexas.Inventory.Application.StockItem.ViewModel;
using Nexas.Inventory.Domain.StockItem.Entity;
using Nexas.Inventory.Infrastructure.StockItem.Interface;

namespace Nexas.Inventory.Application.StockItem.Service;
public class StockItemService : BaseService<StockItemEntity, StockItemViewModel>, IStockItemService
{
    private readonly IStockItemRepository _stockItemRepository;

    public StockItemService(IStockItemRepository stockItemRepository,IMapper mapper) : base(stockItemRepository,mapper)
    {
        _stockItemRepository = stockItemRepository;
    }

    public async Task<IEnumerable<StockItemViewModel>> getNegativeStock()
    {
        var stockItens = await _stockItemRepository.getNegativeStock();
        return _mapper.Map<IEnumerable<StockItemViewModel>>(stockItens);
    }

    public async Task<IEnumerable<StockItemViewModel>> getPositiveStock()
    {
        var stockItens = await _stockItemRepository.getPositiveStock();
        return _mapper.Map<IEnumerable<StockItemViewModel>>(stockItens);
    }

    public async Task<IEnumerable<StockItemViewModel>> getStockByProduct(int id)
    {
        var stockItem = await _stockItemRepository.getStockByProduct(id);
        return _mapper.Map<IEnumerable<StockItemViewModel>>(stockItem);
    }

    public async Task<IEnumerable<StockItemViewModel>> getStockByStore(int id)
    {
        var stockItem = await _stockItemRepository.getStockByStore(id);
        return _mapper.Map<IEnumerable<StockItemViewModel>>(stockItem);
    }
}

