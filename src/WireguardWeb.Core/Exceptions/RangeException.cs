namespace WireguardWeb.Core.Exceptions;

public class RangeException : ArgumentException
{
    public RangeException(ArgumentParameter index, ArgumentParameter count) :
        base(message: $"Invalid value index: {index.Name} -> {index.Param}, \n" +
                      $"its more count: {count.Name} -> {count.Param}") {}
}