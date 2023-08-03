using WireguardWeb.Core.Entities.Base;

namespace WireguardWeb.Core.Repositories.Base;

public interface IRepository<TEntity> 
    where TEntity : IEntity
{
    public int Count { get; }
    public int NextId { get; }
    public void Add(TEntity entity);
    public TEntity GetById(int id);
    public TEntity[] GetAll();
    public TEntity[] GetRange(int index, int count);
    public void Update(TEntity entity);
    public void Remove(int id);
}