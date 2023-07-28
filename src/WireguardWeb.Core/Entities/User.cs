namespace WireguardWeb.Core.Entities;

public class User
{
    public string Id { get; }

    public User(
        string id)
    {
        Id = id;
    }
}