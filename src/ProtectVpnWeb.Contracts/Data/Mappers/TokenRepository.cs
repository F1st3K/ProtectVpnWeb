using ProtectVpnWeb.Core.Repositories;

namespace ProtectVpnWeb.Contracts.Data.Mappers;

public class TokenRepository : ITokenRepository<string>
{
    public string[] GetTokensInRange(int startIndex, int count)
    {
        throw new NotImplementedException();
    }

    public bool TokenExists(string token)
    {
        throw new NotImplementedException();
    }

    public void AddToken(string token)
    {
        throw new NotImplementedException();
    }

    public void RemoveToken(string token)
    {
        throw new NotImplementedException();
    }
}