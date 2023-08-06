using WireguardWeb.Core.Entities.Interfaces;

namespace WireguardWeb.Core.Managers;

public interface IHasher<THasPassword>
    where THasPassword : IHasPassword
{
    public string GetHash(string password);
}