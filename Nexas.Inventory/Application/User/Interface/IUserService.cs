using Nexas.Inventory.Application.Base.Interface;
using Nexas.Inventory.Application.Store.ViewModel;
using Nexas.Inventory.Application.User.ViewModel;
using Nexas.Inventory.Domain.Store.Entity;
using Nexas.Inventory.Domain.User.Entity;

namespace Nexas.Inventory.Application.User.Interface;
public interface IUserService : IBaseService<UserEntity,UserViewModel>
{
    Task<UserViewModel> GetByEmailAsync(string email);
    Task<bool> ValidateUserPassword(string email, string plainPassword);
}

