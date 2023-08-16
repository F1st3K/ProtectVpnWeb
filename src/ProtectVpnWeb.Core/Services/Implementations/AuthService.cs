using ProtectVpnWeb.Core.Dto.User;
using ProtectVpnWeb.Core.Entities;

namespace ProtectVpnWeb.Core.Services.Implementations;

public sealed class AuthService : IAuthService
{
    public string RegisterUser(AuthUserDto dto)
    {
        throw new NotImplementedException();
    }

    public string AuthUser(AuthUserDto dto)
    {
        throw new NotImplementedException();
    }

    public string ChangePassword(ChangePwdDto dto)
    {
        throw new NotImplementedException();
    }

    public (string accessToken, string refreshToken) GetAccessRefreshTokens(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public void RemoveRefreshToken(string token)
    {
        throw new NotImplementedException();
    }

    public bool ValidateAccessToken(string token, out UserRoles? role)
    {
        throw new NotImplementedException();
    }
}