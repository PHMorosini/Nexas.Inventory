using AutoMapper;
using Nexas.Inventory.Application.Base.Service;
using Nexas.Inventory.Application.User.Interface;
using Nexas.Inventory.Application.User.ViewModel;
using Nexas.Inventory.Domain.User.Entity;
using Nexas.Inventory.Infrastructure.User.Interface;

namespace Nexas.Inventory.Infrastructure.User.Service;
public class UserService : BaseService<UserEntity, UserViewModel>, IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, IMapper mapper) : base(userRepository, mapper)
    {
        _userRepository = userRepository;
    }

    public async Task<UserViewModel> GetByEmailAsync(string email)
    {
        var userDb = await _userRepository.GetByEmailAsync(email);
        return _mapper.Map<UserViewModel>(userDb);
    }

    public async Task<bool> ValidateUserPassword(UserViewModel user)
    {
        var userDb = _mapper.Map<UserEntity>(user);
        return await _userRepository.ValidateUserPassword(userDb);
    }
}

