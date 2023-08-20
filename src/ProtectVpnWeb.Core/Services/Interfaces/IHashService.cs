using ProtectVpnWeb.Core.Entities.Interfaces;

namespace ProtectVpnWeb.Core.Services;

public interface IHashService
{
    public string GetHash(string password);
}