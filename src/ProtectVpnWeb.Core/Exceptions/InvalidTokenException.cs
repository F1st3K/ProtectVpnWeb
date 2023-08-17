namespace ProtectVpnWeb.Core.Exceptions;

public class InvalidTokenException : ArgumentException
{
    public InvalidTokenException(ExceptionParameter token) 
        : base(message: $"Token: {token.Name} -> {token.Param} is not valid"){}
}