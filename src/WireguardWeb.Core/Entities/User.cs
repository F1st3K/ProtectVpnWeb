using WireguardWeb.Core.Entities.Interfaces;

namespace WireguardWeb.Core.Entities;

public sealed class User : IEntity, IUniqueNamed
{
    public int Id { get; }

    public string UniqueName { get; }
    
    public User(
        int id,
        string uniqueName)
    {
        Id = id;
        UniqueName = uniqueName;
    }

}