using ProtectVpnWeb.Core.Services;

namespace ProtectVpnWeb.CoreTests.AuthService;

public class MockHashService : IHashService
{
    private string _sol;
    
    public MockHashService(string sol)
    {
        _sol = sol;
    }
    
    public string GetHash(string password)
    {
        return password + _sol;
    }
}