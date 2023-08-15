using ProtectVpnWeb.Core.Entities.Interfaces;

namespace ProtectVpnWeb.Core.Managers;

public interface IHasher<THasPassword>
    where THasPassword : IHasPassword
{
    public string GetHash(string password);
}