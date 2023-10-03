using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Core.Repositories;
using ProtectVpnWeb.Data;

namespace ProtectVpnWeb.Contracts.Data;

public sealed class UserRepository : IRepository<User>, IUniqueNameRepository<User>,
    IManyRelationshipsRepository<User, Connection>
{
    public int Count { get; }
    public int GetNextId()
    {
        throw new NotImplementedException();
    }

    public bool CheckIdUniqueness(int id)
    {
        using var dbContext = new DataContext();
        return dbContext.Users.Any(user => user.Id == id);
    }

    public void Add(User entity)
    {
        throw new NotImplementedException();
    }

    public User GetById(int id)
    {
        throw new NotImplementedException();
    }

    public User[] GetRange(int index, int count)
    {
        throw new NotImplementedException();
    }

    public void Update(User entity)
    {
        throw new NotImplementedException();
    }

    public void Remove(int id)
    {
        throw new NotImplementedException();
    }

    public bool CheckNameUniqueness(string uname)
    {
        throw new NotImplementedException();
    }

    public User GetByUniqueName(string uname)
    {
        throw new NotImplementedException();
    }

    public void Remove(string uname)
    {
        throw new NotImplementedException();
    }

    public Connection[] GetRelatedEntities(User source)
    {
        throw new NotImplementedException();
    }
}