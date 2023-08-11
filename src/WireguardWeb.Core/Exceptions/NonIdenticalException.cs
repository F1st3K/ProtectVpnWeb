namespace WireguardWeb.Core.Exceptions;

public class NonIdenticalException : ArgumentException
{
    public NonIdenticalException(ArgumentParameter expected, ArgumentParameter actual) :
        base(message: $"Expected: {expected.Name} -> {expected.Param} \n" +
                      $"is not equal Actual: {actual.Name} -> {actual.Param}") {}
}