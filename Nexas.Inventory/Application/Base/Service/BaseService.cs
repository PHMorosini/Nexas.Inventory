using AutoMapper;
using Nexas.Inventory.Infrastructure.Base.Interfaces;

namespace Nexas.Inventory.Application.Base.Service
{
    public class BaseService<TEntity, TViewModel>
    where TEntity : class
    where TViewModel : class
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public virtual async Task<TViewModel> GetByIdAsync(object id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return _mapper.Map<TViewModel>(entity);
        }

        public virtual async Task<IEnumerable<TViewModel>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<TViewModel>>(entities);
        }

        public virtual async Task AddAsync(TViewModel vm)
        {
            var entity = _mapper.Map<TEntity>(vm);
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TViewModel vm)
        {
            var entity = _mapper.Map<TEntity>(vm);
            _repository.Update(entity);
            await _repository.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TViewModel vm)
        {
            var entity = _mapper.Map<TEntity>(vm);
            _repository.Delete(entity);
            await _repository.SaveChangesAsync();
        }
    }

}
