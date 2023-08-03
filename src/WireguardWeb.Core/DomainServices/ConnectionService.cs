using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Repositories;

namespace WireguardWeb.Core.DomainServices;

public sealed class ConnectionService<TRepository> 
    where TRepository : IRepository<Connection>
{
    public TRepository ConnectionRepository { get; }
    
    public ConnectionService(TRepository connectionRepository)
    {
        ConnectionRepository = connectionRepository;
    }
}
