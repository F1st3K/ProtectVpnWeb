namespace ProtectVpnWeb.Core.Exceptions;

public class NotRunningException : ArgumentException
{
    public NotRunningException(string name) :
        base(message: $"The {name} is not running") {}
}