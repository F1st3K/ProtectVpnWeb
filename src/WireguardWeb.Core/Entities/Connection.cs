namespace WireguardWeb.Core.Entities;

public class Connection
{
    public string Id { get; }
    public string UserId { get; }

    public Connection(
        string id,
        string userId)
    {
        Id = id;
        UserId = userId;
    }
}