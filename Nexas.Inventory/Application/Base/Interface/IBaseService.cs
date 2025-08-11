namespace Nexas.Inventory.Application.Base.Interface
{
    public interface IBaseService<TEntity, TViewModel>
     where TEntity : class
     where TViewModel : class
    {
        Task<TViewModel> GetByIdAsync(object id);
        Task<IEnumerable<TViewModel>> GetAllAsync();
        Task AddAsync(TViewModel vm);
        Task UpdateAsync(TViewModel vm);
        Task DeleteAsync(TEntity entity);
    }

}
