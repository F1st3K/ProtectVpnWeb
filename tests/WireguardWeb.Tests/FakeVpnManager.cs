using WireguardWeb.Core.Managers;

namespace WireguardWeb.Tests;

public class FakeVpnManager : IVpnManager<FakeClientConnection>
{
    public bool ServerIsActive { get; private set; }

    private List<FakeClientConnection> _connections = new List<FakeClientConnection>();

    public void StartServer()
    {
        ServerIsActive = true;
    }

    public void AddConnection(FakeClientConnection connection)
    {
        _connections.Add(connection);
    }

    public void UpdateConnection(FakeClientConnection connection)
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

    public void RemoveConnection(FakeClientConnection connection)
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
        ServerIsActive = false;
    }
}