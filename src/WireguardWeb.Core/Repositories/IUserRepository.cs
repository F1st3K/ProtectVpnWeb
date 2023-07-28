using WireguardWeb.Core.Entities;

namespace WireguardWeb.Core.Repositories;

public interface IUserRepository
{
    public User GetById(string id);
}