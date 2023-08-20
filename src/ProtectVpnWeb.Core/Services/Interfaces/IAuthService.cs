using ProtectVpnWeb.Core.Dto.User;
using ProtectVpnWeb.Core.Entities;

namespace ProtectVpnWeb.Core.Services.Interfaces;

public interface IAuthService
{
    public string RegisterUser(AuthUserDto dto);

    public string AuthUser(AuthUserDto dto);
    
    public string ChangePassword(ChangePwdDto dto);

    public void GetTokensByRefreshToken(string token, out string refreshToken, out string accessToken);

    public void RemoveRefreshToken(string token);

    public bool ValidateAccessToken(string token, out UserRoles? role);
}