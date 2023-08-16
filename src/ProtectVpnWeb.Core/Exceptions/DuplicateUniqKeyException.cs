namespace ProtectVpnWeb.Core.Exceptions;

public class DuplicateUniqKeyException : Exception
{
    public DuplicateUniqKeyException(ExceptionParameter uKey) :
        base(message: $"Uniq Key: {uKey.Name} -> {uKey.Param} is not unique"){}
}