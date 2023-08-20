using ProtectVpnWeb.Core.Repositories;

namespace ProtectVpnWeb.CoreTests.AuthService;

public class MockTokenRepository : ITokenRepository<string>
{
    private readonly List<string> _tokens = new();
    
    public string[] GetTokensInRange(int startIndex, int count)
    {
        return _tokens.GetRange(startIndex, count).ToArray();
    }

    public bool TokenExists(string token)
    {
        return _tokens.Contains(token);
    }

    public void AddToken(string token)
    {
        _tokens.Add(token);
    }

    public void RemoveToken(string token)
    {
        _tokens.Remove(token);
    }

    public void FakeInit(IEnumerable<string> tokens)
    {
        _tokens.Clear();
        foreach (var t in tokens)
            _tokens.Add(t);
    }
}