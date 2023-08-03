using WireguardWeb.Core.Entities.Interfaces;

namespace WireguardWeb.Core.Repositories;

public interface IUniqueNameRepository<out THasUniqueName> 
    where THasUniqueName : IHasUniqueName
{
    public bool CheckNameUniqueness(string uname);
    public THasUniqueName GetByUniqueName(string uname);
    public void Remove(string uname);
}