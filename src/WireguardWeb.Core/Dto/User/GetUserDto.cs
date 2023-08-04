namespace WireguardWeb.Core.Dto.User;

public class GetUserDto : IDto
{
    public string UserName { get; set; }

    public bool IsValid()
    {
        if (UserName == string.Empty)
            return false;
        return true;
    }
}