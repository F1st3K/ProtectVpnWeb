namespace WireguardWeb.Core.Entities.Base;

public abstract class Entity : IEntity
{
    public int Id { get; }

    public Entity(int id)
    {
        Id = id;
    }
}