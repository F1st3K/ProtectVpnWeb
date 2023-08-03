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
}