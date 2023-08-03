using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Repositories.Base;

namespace WireguardWeb.Core.DomainServices;

public class UserService<TRepository> 
    where TRepository : IRepository<User>
{
    public TRepository UserRepository { get; }

    public UserService(TRepository userRepository)
    {
        UserRepository = userRepository;
    }
}