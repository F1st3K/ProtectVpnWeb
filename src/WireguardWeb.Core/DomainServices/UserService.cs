using WireguardWeb.Core.Repositories.UserRepository;

namespace WireguardWeb.Core.DomainServices;

public class UserService
{
    public IUserRepository UserRepository { get; }

    public UserService(IUserRepository userRepository)
    {
        UserRepository = userRepository;
    }
}