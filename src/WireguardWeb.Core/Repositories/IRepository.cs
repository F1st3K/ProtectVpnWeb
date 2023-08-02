using WireguardWeb.Core.Entities.Base;

namespace WireguardWeb.Core.Repositories.Base;

public interface IRepository<T> where T : IEntity
{
    public int Count { get; }
    public void Add(T entity);
    public T GetById(int id);
    public T[] GetAll();
    public T[] GetRange(int index, int count);
    public void Update(T entity);
    public void Remove(int id);
}