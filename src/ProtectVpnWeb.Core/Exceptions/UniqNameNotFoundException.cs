namespace ProtectVpnWeb.Core.Exceptions;

public class UniqNameNotFoundException : ArgumentException
{
    public UniqNameNotFoundException(ExceptionParameter uname) :
        base(message: $"UniqName: {uname.Name} -> {uname.Param} is not found"){}
}