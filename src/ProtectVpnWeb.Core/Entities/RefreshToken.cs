using ProtectVpnWeb.Core.Entities.Interfaces;

namespace ProtectVpnWeb.Core.Entities;

public class RefreshToken : IEntity, IToken, ITransfer<string>
{
    public int Id { get; }
    
    public string Value { get; private set; }

    public string ToTransfer() => Value;

    public void ChangeOf(string transfer) => Value = transfer;
}