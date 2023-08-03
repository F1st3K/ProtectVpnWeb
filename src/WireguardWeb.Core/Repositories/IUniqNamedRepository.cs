using WireguardWeb.Core.Entities.Base;

namespace WireguardWeb.Core.Repositories.Base;

public interface IUniqNamedRepository<out TUniqueNamed> 
    where TUniqueNamed : IEntity, IUniqueNamed
{
    public bool CheckNameUniqueness(string uname);
    public TUniqueNamed GetByUniqueName(string uname);
    public void Remove(string uname);
}