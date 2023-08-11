namespace WireguardWeb.Core.Exceptions;

public class IdNotFoundException : ArgumentException
{
    public IdNotFoundException(ArgumentParameter id) :
        base(message: $"Id: {id.Name} -> {id.Param} is not found"){}
}