using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Entities.Base;
using WireguardWeb.Core.Repositories.Base;

namespace WireguardWeb.Core.DomainServices;

public class ConnectionService<TRepository> 
    where TRepository : IRepository<Connection>
{
    public TRepository ConnectionRepository { get; }
    
    public ConnectionService(TRepository connectionRepository)
    {
        ConnectionRepository = connectionRepository;
    }
}
