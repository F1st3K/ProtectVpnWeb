using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Managers;
using WireguardWeb.Core.Repositories;

namespace WireguardWeb.Core.DomainServices;

public sealed class ConnectionService<TRepository, TClientConnection> 
    where TRepository : IRepository<Connection>
{
    public TRepository ConnectionRepository { get; }
    
    public IVpnManager<TClientConnection> VpnManager { get; }
    
    public ConnectionService(TRepository connectionRepository,
        IVpnManager<TClientConnection> vpnManager)
    {
        ConnectionRepository = connectionRepository;
        VpnManager = vpnManager;
    }

}
