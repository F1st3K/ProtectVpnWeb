namespace ProtectVpnWeb.Core.Dto.User;

public sealed class ChangePwdDto
{
    public string UserName { get; set; }
    
    public string Password { get; set; }
    
    public string NewPassword { get; set; }
}