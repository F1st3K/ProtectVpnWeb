namespace ProtectVpnWeb.Core.Services;

public interface ITokenService<TToken>
    where TToken : notnull
{
    TToken GenerateToken<TPayload>(TPayload payload, TimeSpan expirationTime);
    bool ValidateToken(TToken token);
    TPayload ReadTokenPayload<TPayload>(TToken token);
}