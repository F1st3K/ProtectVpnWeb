using WireguardWeb.Core.Entities;
using Wireguard.ServiceLibrary.Repositories;

namespace WireguardWeb.Core.Domains;

public class WGConnection
{
    public void Save(ConnectionEntity entity)
    {
        //validate some staff

        var repository = new WGConnectionRepository();
        repository.SaveToDataBase(entity);
    }
}