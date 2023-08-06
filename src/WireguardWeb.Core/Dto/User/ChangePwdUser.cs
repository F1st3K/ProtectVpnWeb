namespace WireguardWeb.Core.Dto.User;

public sealed class ChangePwdUser
{
    public string Password { get; set; }
    
    public string NewPassword { get; set; }
}