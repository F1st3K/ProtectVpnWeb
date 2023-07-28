using WireguardWeb.Core.Repositories.ConnectionRepository;

namespace WireguardWeb.Core.DomainServices;

public class ConnectionService
{
    public IConnectionRepository ConnectionRepository { get; }

    public ConnectionService(IConnectionRepository connectionRepository)
    {
        ConnectionRepository = connectionRepository;
    }
}