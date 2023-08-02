using WireguardWeb.Core.Entities.Base;

namespace WireguardWeb.Core.Entities;

public class Connection : IEntity
{
    public int Id { get; }
    public string UserId { get; }

    public Connection(
        int id,
        string userId)
    {
        Id = id;
        UserId = userId;
    }
}