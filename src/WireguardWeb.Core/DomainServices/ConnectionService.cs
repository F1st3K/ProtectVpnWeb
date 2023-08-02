using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Repositories.Base;

namespace WireguardWeb.Core.DomainServices;

public class ConnectionService
{
    public IRepository<Connection> ConnectionRepository { get; }

    public ConnectionService(IRepository<Connection> connectionRepository)
    {
        ConnectionRepository = connectionRepository;
    }
}