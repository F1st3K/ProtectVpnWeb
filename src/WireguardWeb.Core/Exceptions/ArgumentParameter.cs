namespace WireguardWeb.Core.Exceptions;

public struct ArgumentParameter
{
    public object Param { get; }
    public string Name { get; }

    public ArgumentParameter(object param, string name)
    {
        Param = param;
        Name = name;
    }
}