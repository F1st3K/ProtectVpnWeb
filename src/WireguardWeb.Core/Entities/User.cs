using WireguardWeb.Core.Entities.Base;

namespace WireguardWeb.Core.Entities;

public class User : IEntity, IUniqueNamed
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