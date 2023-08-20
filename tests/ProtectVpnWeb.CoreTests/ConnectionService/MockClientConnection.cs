using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Core.Entities.Interfaces;

namespace ProtectVpnWeb.CoreTests.ConnectionService;

public class MockClientConnection : ITransfer<Connection>
{
    public int Id { get; private set; }
    
    public int UserId { get; private set; }
    
    public int Ip { get; private set; }
    
    public Connection ToTransfer()
    {
        var info = $"IP={Ip}";
        return new Connection(
            Id,
            UserId,
            info);
    }

    public void ChangeOf(Connection transfer)
    {
        Id = transfer.Id;
        UserId = transfer.UserId;
        Ip = Convert.ToInt32(transfer.Info?.Replace("IP=", string.Empty));
    }
}