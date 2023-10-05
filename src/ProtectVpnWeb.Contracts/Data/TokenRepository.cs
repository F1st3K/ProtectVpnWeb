using Microsoft.EntityFrameworkCore;
using ProtectVpnWeb.Core.Repositories;
using ProtectVpnWeb.Data;

namespace ProtectVpnWeb.Contracts.Data;

public class TokenRepository : ITokenRepository<string>
{
    private readonly DataContext _dbContext;
    
    public TokenRepository(DbContextOptions<DataContext> options)
    {
        _dbContext = new DataContext(options);
    }
    
    public string[] GetTokensInRange(int startIndex, int count) =>
        _dbContext.Tokens.Skip(startIndex).Take(count).ToArray();

    public bool TokenExists(string token) =>
        _dbContext.Tokens.Any(t => t == token);

    public void AddToken(string token)
    {
        _dbContext.Tokens.Add(token);
        _dbContext.SaveChanges();
    }

    public void RemoveToken(string token)
    {
        _dbContext.Remove(token);
        _dbContext.SaveChanges();
    }
}