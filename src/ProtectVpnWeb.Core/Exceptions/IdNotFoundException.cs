namespace ProtectVpnWeb.Core.Exceptions;

public class IdNotFoundException : ArgumentException
{
    public IdNotFoundException(ExceptionParameter id) :
        base(message: $"Id: {id.Name} -> {id.Param} is not found"){}
}