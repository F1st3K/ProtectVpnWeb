using WireguardWeb.Core.Entities.Base;

namespace WireguardWeb.Core.Repositories.Base;

public interface IUniqNamedRepository<out TUniqueNamed> 
    where TUniqueNamed : IEntity, IUniqueNamed
{
    public TUniqueNamed GetByUniqueName(string uname);
}