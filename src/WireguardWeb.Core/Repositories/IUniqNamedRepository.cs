using WireguardWeb.Core.Entities.Interfaces;

namespace WireguardWeb.Core.Repositoriesblic interface IUniqNamedRepository<out TUniqueNamed> 
    where TUniqueNamed : IEntity, IUniqueNamed
{
    public bool CheckNameUniqueness(string uname);
    public TUniqueNamed GetByUniqueName(string uname);
    public void Remove(string uname);
}