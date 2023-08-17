namespace ProtectVpnWeb.Core.Exceptions;

public class ParamsMatchException : ArgumentException
{
    public ParamsMatchException(ExceptionParameter first, ExceptionParameter second) :
        base(message: $"First and Second parameters: {first.Name} -> {first.Param} and " +
                      $"{second.Name} -> {second.Param} is match"){}
}