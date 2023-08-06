namespace WireguardWeb.Core.Managers;

public interface IVpnManager<in TConnection> 
{
    public bool ServerIsActive { get; }
    public void StartServer();
    public void AddConnection(TConnection connection);
    public void UpdateConnection(TConnection connection);
    public void RemoveConnection(TConnection connection);
    public string GenerateConnectionInfo();
    public void StopServer();
}