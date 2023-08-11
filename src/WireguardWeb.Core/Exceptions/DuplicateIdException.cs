namespace WireguardWeb.Core.Exceptions;

public class DuplicateIdException : Exception
{
    public DuplicateIdException(ExceptionParameter id) :
        base(message: $"Id: {id.Name} -> {id.Param} is not unique"){}
}