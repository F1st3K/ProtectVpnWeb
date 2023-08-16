using ProtectVpnWeb.Core.Dto.User;
using ProtectVpnWeb.Core.Entities;

namespace ProtectVpnWeb.Core.Services;

public interface IAuthService
{
    public string RegisterUser(AuthUserDto dto);

    public string AuthUser(AuthUserDto dto);
    
    public string ChangePassword(ChangePwdDto dto);

    public (string accessToken, string refreshToken) GetAccessRefreshTokens(string refreshToken);

    public void RemoveRefreshToken(string token);

    public bool ValidateAccessToken(string token, out UserRoles? role);
}