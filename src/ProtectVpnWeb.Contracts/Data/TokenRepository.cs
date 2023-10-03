using ProtectVpnWeb.Core.Repositories;
using ProtectVpnWeb.Data;

namespace ProtectVpnWeb.Contracts.Data;

public class TokenRepository : ITokenRepository<string>
{
    public string[] GetTokensInRange(int startIndex, int count)
    {
        using var dbContext = new DataContext();
        return dbContext.Tokens.Skip(startIndex).Take(count).ToArray();
    }

    public bool TokenExists(string token)
    {
        using var dbContext = new DataContext();
        return dbContext.Tokens.Any(t => t == token);
    }

    public void AddToken(string token)
    {
        using var dbContext = new DataContext();
        dbContext.Tokens.Add(token);
        dbContext.SaveChanges();
    }

    public void RemoveToken(string token)
    {
        using var dbContext = new DataContext();
        dbContext.Remove(token);
        dbContext.SaveChanges();
    }
}