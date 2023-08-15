using ProtectVpnWeb.Core.Entities.Interfaces;

namespace ProtectVpnWeb.Core.Services;

public interface IHashService<THasPassword>
    where THasPassword : IHasPassword
{
    public string GetHash(string password);
}