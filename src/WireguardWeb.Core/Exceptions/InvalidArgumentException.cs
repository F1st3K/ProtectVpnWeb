using System.Text;

namespace WireguardWeb.Core.Exceptions;

public class InvalidArgumentException : ArgumentException
{
    public InvalidArgumentException(params ArgumentParameter[] parameters) :
        base(message: CreateMessage(parameters)) {}

    private static string CreateMessage(params ArgumentParameter[] parameters)
    {
        var msg = new StringBuilder("Invalid data:");
        foreach (var parameter in parameters)
            msg.AppendLine($"\t{parameter.Name} -> {parameter.Param.ToString()}");
        return msg.ToString();
    }
}