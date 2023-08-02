using WireguardWeb.Core.Entities.Base;

namespace WireguardWeb.Core.Entities;

public class Connection : Entity
{
    public string UserId { get; }

    public Connection(int id, string userId)
        : base(id)
    {
        UserId = userId;
    }
}