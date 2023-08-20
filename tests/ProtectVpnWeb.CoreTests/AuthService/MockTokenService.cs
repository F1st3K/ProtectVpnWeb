using ProtectVpnWeb.Core.Services;
using ProtectVpnWeb.Core.Services.Interfaces;

namespace ProtectVpnWeb.CoreTests.AuthService;

public class MockTokenService : ITokenService<string>
{
    private readonly Dictionary<string, object> _objects = new();
    
    public string GenerateToken<TPayload>(TPayload payload, TimeSpan expirationTime)
    {
        var token = Guid.NewGuid().ToString();
        if (payload != null) _objects.Add(token, payload);
        return token;
    }

    public bool ValidateToken(string token)
    {
        return _objects.ContainsKey(token);
    }

    public TPayload ReadTokenPayload<TPayload>(string token)
    {
        if (_objects.TryGetValue(token, out var obj) &&
            obj is TPayload payload)
            return payload;
        throw new InvalidOperationException("Invalid token or payload type (Mock)");
    }

    public void FakeInit<TPayload>(string[] tokens, TPayload[] payloads)
    {
        if (tokens.Length != payloads.Length)
            throw new InvalidOperationException("Count tokens and payloads is not equals");
        _objects.Clear();
        for (int i = 0; i < tokens.Length; i++)
            _objects.Add(tokens[i], payloads[i]!);
    }
}