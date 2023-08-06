using WireguardWeb.Core.Dto.Connection;
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

    public void Restart()
    {
        if (VpnManager.ServerIsActive) 
            VpnManager.StopServer();
        
        VpnManager.StartServer();
        
        var connections = ConnectionRepository.GetRange(0, ConnectionRepository.Count);
        foreach (var connection in connections)
        {
            var clientConnection = new TClientConnection();
            clientConnection.ChangeOf(connection);
            VpnManager.AddConnection(clientConnection);
        }
    }

    public ConnectionDto GetConnection(int id)
    {
        if (id < 0)
            throw new Exception("Invalid data");

        if (ConnectionRepository.CheckIdUniqueness(id))
            throw new Exception("Transferred Id is not find");

        var connection = ConnectionRepository.GetById(id);

        return connection.ToTransfer();
    }

    public ConnectionDto[] GetUsersInRange(int startIndex, int count)
    {
        if (startIndex < 0 || count <= 0)
            throw new Exception("Invalid data");

        if (startIndex + count > ConnectionRepository.Count)
            throw new Exception($"Transferred startIndex and count:{startIndex + count}" +
                                $" was more than there are count users:{ConnectionRepository.Count}");

        var connections = ConnectionRepository.GetRange(startIndex, count);
        
        var connectionsDto = new ConnectionDto[connections.Length];
        for (int i = 0; i < connections.Length; i++)
        {
            connectionsDto[i] = connections[i].ToTransfer();
        }
        return connectionsDto;
    }

    public void CreateConnection(CreateConnectionDto dto)
    {
        if (VpnManager.ServerIsActive == false)
            throw new Exception("Vpn server is not running");

        if (dto.UserId < 0)
            throw new Exception("Invalid data");

        string info = VpnManager.GenerateConnectionInfo();
        var newConnection = new Connection(ConnectionRepository.GetNextId(), dto.UserId, info);
        ConnectionRepository.Add(newConnection);
    }

    public void EditConnection(int id, ConnectionDto dto)
    {
        if (id < 0)
            throw new Exception("Invalid data");

        if (ConnectionRepository.CheckIdUniqueness(id))
            throw new Exception("Transferred Id is not find");

        var connection = ConnectionRepository.GetById(id);

        if (connection.Id != dto.Id)
            throw new Exception("Id is not be changed");
        
        connection.ChangeOf(dto);
        ConnectionRepository.Update(connection); 
        
        var clientConnection = new TClientConnection();
        clientConnection.ChangeOf(connection);
        VpnManager.UpdateConnection(clientConnection);
    }

    public void RemoveConnection(int id)
    {
        if (id < 0)
            throw new Exception("Invalid data");

        if (ConnectionRepository.CheckIdUniqueness(id))
            throw new Exception("Transferred Id is not find");

        var connection = ConnectionRepository.GetById(id);
        
        ConnectionRepository.Remove(connection.Id);
        
        var clientConnection = new TClientConnection();
        clientConnection.ChangeOf(connection);
        VpnManager.RemoveConnection(clientConnection);
    }
}
