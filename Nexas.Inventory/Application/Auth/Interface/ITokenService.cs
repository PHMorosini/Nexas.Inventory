using Nexas.Inventory.Domain.User.Entity;

namespace Nexas.Inventory.Application.Auth.Interface;
public interface ITokenService
{
     Task<string> GenerateToken(UserEntity user);
}
