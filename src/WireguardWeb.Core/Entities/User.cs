using WireguardWeb.Core.Entities.Interfaces;

namespace WireguardWeb.Core.Entities;

public sealed class User : IEntity, IUniqueNamed
{
    public int Id { get; }

    public string UniqueName { get; }
    
    public string HashPassword { get; }
    
    public User(
        int id,
        string uniqueName,
        string hashPassword)
    {
        Id = id;
        UniqueName = uniqueName;
        HashPassword = hashPassword;
    }

}