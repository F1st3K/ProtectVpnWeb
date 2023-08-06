using WireguardWeb.Core.Entities.Interfaces;

namespace WireguardWeb.Core.Repositories;

public interface IRepository<TEntity> 
    where TEntity : IEntity
{
    public int Count { get; }
    public int GetNextId();
    public bool CheckIdUniqueness(int id);
    public void Add(TEntity entity);
    public TEntity GetById(int id);
    public TEntity[] GetRange(int index, int count);
    public void Update(TEntity entity);
    public void Remove(int id);
}