using System.Text;

namespace ProtectVpnWeb.Core.Exceptions;

public class InvalidArgumentException : ArgumentException
{
    public InvalidArgumentException(params ExceptionParameter[] parameters) :
        base(message: CreateMessage(parameters)) {}

    private static string CreateMessage(params ExceptionParameter[] parameters)
    {
        var msg = new StringBuilder("Invalid data:");
        foreach (var parameter in parameters)
            msg.AppendLine($"\t{parameter.Name} -> {parameter.Param.ToString()}");
        return msg.ToString();
    }
}