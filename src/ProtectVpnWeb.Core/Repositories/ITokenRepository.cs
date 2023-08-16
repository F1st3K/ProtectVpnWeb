using ProtectVpnWeb.Core.Entities.Interfaces;

namespace ProtectVpnWeb.Core.Repositories;

public interface ITokenRepository<TToken>
    where TToken : notnull
{
    public TToken[] GetTokensInRange(int startIndex, int count);

    public bool TokenExists(TToken token);

    public void RemoveToken(TToken token);
}