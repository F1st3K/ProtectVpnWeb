using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Repositories.Base;

namespace WireguardWeb.Core.DomainServices;

public class UserService
{
    public IRepository<User> UserRepository { get; }

    public UserService(IRepository<User> userRepository)
    {
        UserRepository = userRepository;
    }
}