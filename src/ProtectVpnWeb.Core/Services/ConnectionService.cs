using ProtectVpnWeb.Core.Dto.Connection;
using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Core.Entities.Interfaces;
using ProtectVpnWeb.Core.Exceptions;
using ProtectVpnWeb.Core.Repositories;
using ProtectVpnWeb.Core.Services.Interfaces;

namespace ProtectVpnWeb.Core.Services;

public sealed class ConnectionService<TRepository, TClientConnection, TVpnManager> : IConnectionService
    where TRepository : IRepository<Connection>
    where TClientConnection : ITransfer<Connection>, new()
    where TVpnManager : IVpnService<TClientConnection>
{
    private TRepository ConnectionRepository { get; }
    
    private TVpnManager VpnManager { get; }
    
    public ConnectionService(
        TRepository connectionRepository,
        TVpnManager vpnManager)
    {
        ConnectionRepository = connectionRepository;
        VpnManager = vpnManager;
    }

    public void Restart()
    {
        if (VpnManager.ServerIsActive) 
            VpnManager.StopServer();
        
        VpnManager.StartServer();
        
        var connections = 
            ConnectionRepository.GetRange(0, ConnectionRepository.Count);
        foreach (var c in connections)
        {
            var clientConnection = new TClientConnection();
            clientConnection.ChangeOf(c);
            VpnManager.AddConnection(clientConnection);
        }
    }

    public ConnectionDto GetConnection(int id)
    {
        if (id < 0)
            throw new InvalidArgumentException(
                new ExceptionParameter(id, nameof(id)));

        if (ConnectionRepository.CheckIdUniqueness(id))
            throw new NotFoundException(
                new ExceptionParameter(id, nameof(id)));

        var connection = ConnectionRepository.GetById(id);

        return connection.ToTransfer();
    }

    public ConnectionDto[] GetConnectionsInRange(int startIndex, int count)
    {
        if (startIndex < 0 || count <= 0)
            throw new InvalidArgumentException(
                new ExceptionParameter(startIndex, nameof(startIndex)),
                new ExceptionParameter(count, nameof(count)));

        if (startIndex + count > ConnectionRepository.Count)
            throw new RangeException(
                new ExceptionParameter(startIndex+count, 
                    nameof(startIndex) + "+" + nameof(count)),
                new ExceptionParameter(ConnectionRepository.Count, 
                    nameof(ConnectionRepository.Count)));

        var connections = ConnectionRepository.GetRange(startIndex, count);
        
        var connectionsDto = new ConnectionDto[connections.Length];
        for (int i = 0; i < connections.Length; i++)
        {
            connectionsDto[i] = connections[i].ToTransfer();
        }
        return connectionsDto;
    }

    public ConnectionDto CreateConnection(CreateConnectionDto dto)
    {
        if (dto.UserId < 0)
            throw new InvalidArgumentException(
            new ExceptionParameter(dto.UserId, nameof(dto.UserId)));
        
        if (VpnManager.ServerIsActive == false)
            throw new NotRunningException(nameof(VpnManager));

        string info = VpnManager.GenerateConnectionInfo();
        var newConnection = new Connection(ConnectionRepository.GetNextId(), dto.UserId, info);
        ConnectionRepository.Add(newConnection);
        
        var newClientConnection = new TClientConnection();
        newClientConnection.ChangeOf(newConnection);
        VpnManager.AddConnection(newClientConnection);
        
        return newConnection.ToTransfer();
    }

    public void EditConnection(int id, ConnectionDto dto)
    {
        if (id < 0)
            throw new InvalidArgumentException(
                new ExceptionParameter(id, nameof(id)));
        if (dto.UserId < 0 ||
            dto.Id < 0 ||
            dto.Info == string.Empty)
            throw new InvalidArgumentException(
                new ExceptionParameter(dto.UserId, nameof(dto.UserId)),
                new ExceptionParameter(dto.Id, nameof(dto.Id)),
                new ExceptionParameter(dto.Info, nameof(dto.Info)));

        if (ConnectionRepository.CheckIdUniqueness(id))
            throw new NotFoundException(
            new ExceptionParameter(id, nameof(id)));

        var connection = ConnectionRepository.GetById(id);

        if (connection.Id != dto.Id)
            throw new NonIdenticalException(
                new ExceptionParameter(connection.Id, nameof(connection.Id)),
                    new ExceptionParameter(dto.Id, nameof(dto.Id)));
        
        connection.ChangeOf(dto);
        ConnectionRepository.Update(connection); 
        
        var clientConnection = new TClientConnection();
        clientConnection.ChangeOf(connection);
        VpnManager.UpdateConnection(clientConnection);
    }

    public void RemoveConnection(int id)
    {
        if (id < 0)
            throw new InvalidArgumentException(
                new ExceptionParameter(id, nameof(id)));

        if (ConnectionRepository.CheckIdUniqueness(id))
            throw new NotFoundException(
                new ExceptionParameter(id, nameof(id)));

        var connection = ConnectionRepository.GetById(id);
        
        ConnectionRepository.Remove(connection.Id);
        
        var clientConnection = new TClientConnection();
        clientConnection.ChangeOf(connection);
        VpnManager.RemoveConnection(clientConnection);
    }
}
