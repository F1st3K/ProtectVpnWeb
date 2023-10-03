using ProtectVpnWeb.Contracts.Data.Mappers;
using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Core.Repositories;
using ProtectVpnWeb.Data;
using ProtectVpnWeb.Data.Entities;

namespace ProtectVpnWeb.Contracts.Data;

public sealed class UserRepository : IRepository<User>, IUniqueNameRepository<User>,
    IManyRelationshipsRepository<User, Connection>
{
    private readonly IMapper<User, UserEntity> _userMapper = new UserMapper();
    private readonly IMapper<Connection, ConnectionEntity> _connectionMapper = new ConnectionMapper();
    
    public int Count
    {
        get
        {
            using var dbContext = new DataContext();
            return dbContext.Users.Count();
        }
    }

    public int GetNextId()
    {
        using var dbContext = new DataContext();
        return dbContext.Users.Max(user => user.Id) + 1;
    }

    public bool CheckIdUniqueness(int id)
    {
        using var dbContext = new DataContext();
        return !dbContext.Users.Any(user => user.Id == id);
    }

    public void Add(User entity)
    {
        var user = _userMapper.ToData(entity);
        using var dbContext = new DataContext();
        dbContext.Users.Add(user);
        dbContext.SaveChanges();
    }

    public User GetById(int id)
    {
        using var dbContext = new DataContext();
        var user = dbContext.Users.First(user => user.Id == id);
        return _userMapper.ToDomain(user);
    }

    public User[] GetRange(int index, int count)
    {
        using var dbContext = new DataContext();
        var users = dbContext.Users.Skip(index).Take(count).ToList();
        return users.ConvertAll(user => _userMapper.ToDomain(user)).ToArray();
    }

    public void Update(User entity)
    {
        var user = _userMapper.ToData(entity);
        using var dbContext = new DataContext();
        dbContext.Users.Update(user);
        dbContext.SaveChanges();
    }

    public void Remove(int id)
    {
        using var dbContext = new DataContext();
        dbContext.Users.Remove(dbContext.Users.First(user => user.Id == id));
        dbContext.SaveChanges();
    }

    public bool CheckNameUniqueness(string uname)
    {
        using var dbContext = new DataContext();
        return !dbContext.Users.Any(user => user.UniqueName == uname);
    }

    public User GetByUniqueName(string uname)
    {
        using var dbContext = new DataContext();
        var user = dbContext.Users.First(user => user.UniqueName == uname);
        return _userMapper.ToDomain(user);
    }

    public void Remove(string uname)
    {
        using var dbContext = new DataContext();
        dbContext.Users.Remove(dbContext.Users.First(user => user.UniqueName == uname));
        dbContext.SaveChanges();
    }

    public Connection[] GetRelatedEntities(User source)
    {
        using var dbContext = new DataContext();
        var user = _userMapper.ToData(source);
        var connections =
            dbContext.Connections.Where(connection => connection.UserId == user.Id).ToList();
        return connections.ConvertAll(connection => _connectionMapper.ToDomain(connection)).ToArray();
    }
}