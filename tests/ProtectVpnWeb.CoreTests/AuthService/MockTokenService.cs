using ProtectVpnWeb.Core.Services;

namespace ProtectVpnWeb.CoreTests.AuthService;

public class MockTokenService : ITokenService<string>
{
    public string GenerateToken<TPayload>(TPayload payload, TimeSpan expirationTime)
    {
        throw new NotImplementedException();
    }

    public bool ValidateToken(string token)
    {
        throw new NotImplementedException();
    }

    public TPayload ReadTokenPayload<TPayload>(string token)
    {
        throw new NotImplementedException();
    }
}