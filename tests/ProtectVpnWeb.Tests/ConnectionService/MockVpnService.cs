using ProtectVpnWeb.Core.Services;

namespace ProtectVpnWeb.Tests.ConnectionService;

public class MockVpnService : IVpnService<MockClientConnection>
{
    public bool ServerIsActive { get; private set; }
    
    private readonly List<MockClientConnection> _connections = new();

    public void StartServer()
    {
        ServerIsActive = true;
    }

    public void AddConnection(MockClientConnection connection)
    {
        _connections.Add(connection);
    }

    public void UpdateConnection(MockClientConnection connection)
    {
        foreach (var c in _connections)
        {
            if (c.Id == connection.Id)
            {
                _connections.Remove(c);
                _connections.Add(connection);
                return;
            }
        }
    }

    public void RemoveConnection(MockClientConnection connection)
    {
        foreach (var c in _connections)
        {
            if (c.Id == connection.Id)
            {
                _connections.Remove(c);
                return;
            }
        }
    }

    public string GenerateConnectionInfo()
    {
        return "IP=" + _connections.Count;
    }

    public void StopServer()
    {
        Clear();
        ServerIsActive = false;
    }

    public void Clear()
    {
        _connections.Clear();
    }

    public MockClientConnection GetById(int id)
    {
        foreach (var fc in _connections)
        {
            if (id == fc.Id)
                return fc;
        }

        return null;
    }
}