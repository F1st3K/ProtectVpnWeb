using WireguardWeb.Core.Entities.Base;

namespace WireguardWeb.Core.Entities;

public class User : IEntity
{
    public int Id { get; }

    public User(
        int id)
    {
        Id = id;
    }
}