using ProtectVpnWeb.Core.Services;
using ProtectVpnWeb.Core.Services.Interfaces;

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