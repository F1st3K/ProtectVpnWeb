using ProtectVpnWeb.Core.Dto.User;

namespace ProtectVpnWeb.Core.Services;

public interface IAuthService
{
    public string RegisterUser(AuthUserDto dto);

    public string AuthUser(AuthUserDto dto);
    
    public string ChangePassword(ChangePwdDto dto);

    public Tuple<string, string> GetAccessRefreshTokens(string refreshToken);

    public void RemoveRefreshToken(string token);

    public bool ValidateAccessToken(string token);
}