using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Data.Entities;

namespace ProtectVpnWeb.Contracts.Data.Mappers;

public class ConnectionMapper : IMapper<Connection, ConnectionEntity>
{
    public Connection ToDomain(ConnectionEntity entity)
    {
        return new Connection(
            entity.Id,
            entity.UserId,
            entity.Info
            );
    }

    public ConnectionEntity ToData(Connection domain)
    {
        return new ConnectionEntity 
        {
            Id = domain.Id,
            UserId = domain.UserId,
            Info = domain.Info
        };
    }
}