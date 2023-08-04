namespace WireguardWeb.Core.Dto.User;

public sealed class CreateUserDto : IDto
{
    public string UserName { get; set; }
    
    public string Password { get; set; }
    
    public bool IsValid()
    {
        if (UserName == string.Empty || Password == string.Empty)
            return false;
        return true;
    }
}