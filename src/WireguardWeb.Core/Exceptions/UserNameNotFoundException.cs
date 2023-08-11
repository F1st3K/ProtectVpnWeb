namespace WireguardWeb.Core.Exceptions;

public class UserNameNotFoundException : ArgumentException
{
    public UserNameNotFoundException(ExceptionParameter id) :
        base(message: $"UserName: {id.Name} -> {id.Param} is not found"){}
}