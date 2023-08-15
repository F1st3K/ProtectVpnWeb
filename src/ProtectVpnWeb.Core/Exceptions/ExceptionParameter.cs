namespace ProtectVpnWeb.Core.Exceptions;

public struct ExceptionParameter
{
    public object Param { get; }
    public string Name { get; }

    public ExceptionParameter(object param, string name)
    {
        Param = param;
        Name = name;
    }
}