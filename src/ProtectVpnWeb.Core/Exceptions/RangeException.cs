namespace ProtectVpnWeb.Core.Exceptions;

public class RangeException : ArgumentException
{
    public RangeException(ExceptionParameter index, ExceptionParameter count) :
        base(message: $"Invalid value index: {index.Name} -> {index.Param}, \n" +
                      $"its more count: {count.Name} -> {count.Param}") {}
}