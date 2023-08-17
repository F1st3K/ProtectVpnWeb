namespace ProtectVpnWeb.Core.Services;

public interface ITokenService<TToken>
    where TToken : notnull
{
    public TToken GenerateToken();
}