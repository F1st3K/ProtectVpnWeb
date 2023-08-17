namespace ProtectVpnWeb.Core.Exceptions;

public class InvalidAuthenticationException : ArgumentException
{
    public InvalidAuthenticationException() :
        base(message: "Invalid Authentication: incorrect username or password"){}
}