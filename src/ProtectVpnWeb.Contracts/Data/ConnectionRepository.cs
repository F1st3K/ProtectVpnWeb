using ProtectVpnWeb.Contracts.Data.Mappers;
using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Core.Repositories;
using ProtectVpnWeb.Data;
using ProtectVpnWeb.Data.Entities;

namespace ProtectVpnWeb.Contracts.Data;

public class ConnectionRepository : IRepository<Connection>,
    IOneRelationshipRepository<Connection, User>
{
    private readonly IMapper<User, UserEntity> _userMapper = new UserMapper();
    private readonly IMapper<Connection, ConnectionEntity> _connectionMapper = new ConnectionMapper();
    
    public int Count
    {
        get
        {
            using var dbContext = new DataContext();
            return dbContext.Connections.Count();
        }
    }

    public int GetNextId()
    {
        using var dbContext = new DataContext();
        return dbContext.Connections.Max(connection => connection.Id) + 1;
    }

    public bool CheckIdUniqueness(int id)
    {
        using var dbContext = new DataContext();
        return !dbContext.Connections.Any(connection => connection.Id == id);
    }

    public void Add(Connection entity)
    {
        var connection = _connectionMapper.ToData(entity);
        using var dbContext = new DataContext();
        dbContext.Connections.Add(connection);
        dbContext.SaveChanges();
    }

    public Connection Get(int id)
    {
        using var dbContext = new DataContext();
        var connection = dbContext.Connections.First(connection => connection.Id == id);
        return _connectionMapper.ToDomain(connection);
    }

    public Connection[] GetRange(int index, int count)
    {
        using var dbContext = new DataContext();
        var connections = dbContext.Connections.Skip(index).Take(count).ToList();
        return connections.ConvertAll(connection => _connectionMapper.ToDomain(connection)).ToArray();
    }

    public void Update(Connection entity)
    {
        var connection = _connectionMapper.ToData(entity);
        using var dbContext = new DataContext();
        dbContext.Connections.Update(connection);
        dbContext.SaveChanges();
    }

    public void Remove(int id)
    {
        using var dbContext = new DataContext();
        dbContext.Connections.Remove(dbContext.Connections.First(connection => connection.Id == id));
        dbContext.SaveChanges();
    }

    public User GetRelatedEntity(Connection source)
    {
        using var dbContext = new DataContext();
        var connection = _connectionMapper.ToData(source);
        var user = dbContext.Users.First(user => user.Id == connection.UserId);
        return _userMapper.ToDomain(user);
    }
}