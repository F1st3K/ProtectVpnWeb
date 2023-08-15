namespace ProtectVpnWeb.Core.Exceptions;

public class NonIdenticalException : ArgumentException
{
    public NonIdenticalException(ExceptionParameter expected, ExceptionParameter actual) :
        base(message: $"Expected: {expected.Name} -> {expected.Param} \n" +
                      $"is not equal Actual: {actual.Name} -> {actual.Param}") {}
}