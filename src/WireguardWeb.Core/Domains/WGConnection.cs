using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Repositories;

namespace WireguardWeb.Core.Domains;

public class WGConnection
{
    public void Save(Connection entity)
    {
        //validate some staff

        var repository = new WGConnectionRepository();
        repository.SaveToDataBase(entity);
    }
}