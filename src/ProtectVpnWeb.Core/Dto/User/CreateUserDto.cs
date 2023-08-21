namespace ProtectVpnWeb.Core.Dto.User;

public class CreateUserDto
{
    public string UniqueName { get; set; }
    
    public string Role { get; set; }
    
    public string Password { get; set; }
}