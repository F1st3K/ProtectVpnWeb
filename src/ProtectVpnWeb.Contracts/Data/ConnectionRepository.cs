using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Core.Repositories;

namespace ProtectVpnWeb.Contracts.Data;

public class ConnectionRepository : IRepository<Connection>,
    IOneRelationshipRepository<Connection, User>
{
    public int Count { get; }
    public int GetNextId()
    {
        throw new NotImplementedException();
    }

    public bool CheckIdUniqueness(int id)
    {
        throw new NotImplementedException();
    }

    public void Add(Connection entity)
    {
        throw new NotImplementedException();
    }

    public Connection GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Connection[] GetRange(int index, int count)
    {
        throw new NotImplementedException();
    }

    public void Update(Connection entity)
    {
        throw new NotImplementedException();
    }

    public void Remove(int id)
    {
        throw new NotImplementedException();
    }

    public User GetRelatedEntity(Connection source)
    {
        throw new NotImplementedException();
    }
}