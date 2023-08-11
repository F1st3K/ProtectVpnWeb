namespace WireguardWeb.Core.Exceptions;

public class UserNameNotFoundException : ArgumentException
{
    public UserNameNotFoundException(ArgumentParameter id) :
        base(message: $"UserName: {id.Name} -> {id.Param} is not found"){}
}