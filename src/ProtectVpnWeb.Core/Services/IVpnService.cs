namespace ProtectVpnWeb.Core.Services;

public interface IVpnService<in TConnection> 
{
    public bool ServerIsActive { get; }
    public void StartServer();
    public void AddConnection(TConnection connection);
    public void UpdateConnection(TConnection connection);
    public void RemoveConnection(TConnection connection);
    public string GenerateConnectionInfo();
    public void StopServer();
}