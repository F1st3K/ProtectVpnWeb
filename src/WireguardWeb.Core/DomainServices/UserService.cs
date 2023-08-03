using WireguardWeb.Core.Dto.User;
using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Managers;
using WireguardWeb.Core.Repositories;

namespace WireguardWeb.Core.DomainServices;

public sealed class UserService<TRepository> 
    where TRepository : IRepository<User>, IUniqueNameRepository<User>
{
    public TRepository UserRepository { get; }

    public UserService(TRepository userRepository)
    {
        UserRepository = userRepository;
    }

    public void CreateUser<TPasswordManager>(CreateUserDto dto, TPasswordManager manager)
        where TPasswordManager : IPasswordManager<User>
    {
        if (UserRepository.CheckNameUniqueness(dto.UserName) == false)
            throw new Exception("Transferred UserName is not unique");

        var pwdHash = manager.GetHash(dto.Password);
        var user = new User(UserRepository.NextId, dto.UserName, pwdHash);
        UserRepository.Add(user);
    }
}