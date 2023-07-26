using Wireguard.ServiceLibrary.Entities;
using Wireguard.ServiceLibrary.Repositories;

namespace Wireguard.ServiceLibrary.Domains;

public class WGConnection
{
    public void Save(WGConnectionEntity entity)
    {
        //validate some staff

        var repository = new WGConnectionRepository();
        repository.SaveToDataBase(entity);
    }
}