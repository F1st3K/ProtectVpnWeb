namespace WireguardWeb.Core.Dto.User;

public sealed class CreateUserDto
{
    public string UserName { get; set; }
    
    public string Password { get; set; }
}