namespace ProtectVpnWeb.Data.Entities;

public sealed class ConnectionEntity : BaseEntity
{
    public int UserId { get; set; }
    public string Info { get; set; }
}