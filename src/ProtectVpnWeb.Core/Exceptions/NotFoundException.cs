namespace ProtectVpnWeb.Core.Exceptions;

public class NotFoundException : ArgumentException
{
    public NotFoundException(ExceptionParameter parameter) :
        base(message: $"Parameter: {parameter.Name} -> {parameter.Param} is not found"){}
}