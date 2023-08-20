using ProtectVpnWeb.Core.Services;

namespace ProtectVpnWeb.CoreTests.AuthService;

public class MockHashService : IHashService
{
    public string GetHash(string password)
    {
        return password + "//hashed";
    }
}