using WireguardWeb.Core.Dto.User;
using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Repositories;

namespace WireguardWeb.Core.DomainServices;

public sealed class UserService<TRepository> 
    where TRepository : IRepository<User>, IUniqNamedRepository<User>
{
    public TRepository UserRepository { get; }

    public UserService(TRepository userRepository)
    {
        UserRepository = userRepository;
    }

    public void CreateUser(CreateUserDto dto)
    {
        if (UserRepository.CheckNameUniqueness(dto.UserName) == false)
            throw new Exception("Transferred UserName is not unique");
        
        var user = new User(UserRepository.NextId, dto.UserName, dto.Password);
        UserRepository.Add(user);
    }
}