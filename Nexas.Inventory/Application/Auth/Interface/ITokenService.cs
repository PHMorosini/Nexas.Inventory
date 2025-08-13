using Nexas.Inventory.Application.User.ViewModel;
using Nexas.Inventory.Domain.User.Entity;

namespace Nexas.Inventory.Application.Auth.Interface;
public interface ITokenService
{
     Task<string> GenerateToken(UserViewModel user);
}
