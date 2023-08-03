using WireguardWeb.Core.Entities.Interfaces;

namespace WireguardWeb.Core.Entities;

public sealed class Connection : IEntity
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