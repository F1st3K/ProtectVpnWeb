namespace ProtectVpnWeb.Core.Services.Interfaces;

public interface IHashService
{
    public string GetHash(string password);
}