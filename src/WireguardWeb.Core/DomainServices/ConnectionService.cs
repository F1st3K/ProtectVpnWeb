using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Entities.Interfaces;
using WireguardWeb.Core.Managers;
using WireguardWeb.Core.Repositories;

namespace WireguardWeb.Core.DomainServices;

public sealed class ConnectionService<TRepository, TClientConnection> 
    where TRepository : IRepository<Connection>
    where TClientConnection : ITransfer<Connection>, new()
{
    public TRepository ConnectionRepository { get; }
    
    public IVpnManager<TClientConnection> VpnManager { get; }
    
    public ConnectionService(
        TRepository connectionRepository,
        IVpnManager<TClientConnection> vpnManager)
    {
        ConnectionRepository = connectionRepository;
        VpnManager = vpnManager;
    }

    public void Start()
    {
        VpnManager.StartServer();
        var connections = ConnectionRepository.GetRange(0, ConnectionRepository.Count);
        foreach (var connection in connections)
        {
            var clientConnection = new TClientConnection();
            clientConnection.ChangeOf(connection);
            VpnManager.AddConnection(clientConnection);
        }
    }
}
