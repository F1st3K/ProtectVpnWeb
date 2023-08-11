namespace WireguardWeb.Core.Exceptions;

public class DuplicateIdException : Exception
{
    public DuplicateIdException(ArgumentParameter id) :
        base(message: $"Id: {id.Name} -> {id.Param} is not unique"){}
}