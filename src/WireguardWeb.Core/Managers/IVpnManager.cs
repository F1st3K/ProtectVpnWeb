using WireguardWeb.Core.Repositories;

namespace WireguardWeb.Core.Managers;

public interface IVpnManager<TConnection> 
{
    public void StartServer();
    public void AddConnection(TConnection connection);
    public void RemoveConnection(TConnection connection);
    public void UpdateConnection(TConnection connection);
    public void StopServer();
}