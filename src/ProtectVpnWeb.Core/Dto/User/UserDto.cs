namespace ProtectVpnWeb.Core.Dto.User;

public sealed class UserDto
{
    public int Id { get; set; }

    public string UniqueName { get; set; }
    
    public string Role { get; set; }
}