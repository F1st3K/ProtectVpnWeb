using ProtectVpnWeb.Core.Entities.Interfaces;

namespace ProtectVpnWeb.Core.Repositories;

public interface IUniqueNameRepository<out THasUniqueName> 
    where THasUniqueName : IHasUniqueName
{
    public bool CheckNameUniqueness(string uname);
    public THasUniqueName GetByUniqueName(string uname);
    public void Remove(string uname);
}