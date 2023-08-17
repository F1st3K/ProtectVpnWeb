namespace ProtectVpnWeb.Core.Dto.Auth;

public class TimeLiveTokensDto
{
    public TimeSpan RefreshAuthToken { get; set; }
    
    public TimeSpan RefreshToken { get; set; }
    
    public TimeSpan AccessToken { get; set; }
}