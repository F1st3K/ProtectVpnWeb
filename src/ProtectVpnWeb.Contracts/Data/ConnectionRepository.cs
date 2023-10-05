using Microsoft.EntityFrameworkCore;
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
    private readonly DataContext _dbContext;
    
    public ConnectionRepository(DbContextOptions<DataContext> options)
    {
        _dbContext = new DataContext(options);
    }
    
    public int Count => _dbContext.Connections.Count();

    public int GetNextId() =>
        _dbContext.Connections.Max(connection => connection.Id) + 1;

    public bool CheckIdUniqueness(int id) =>
        !_dbContext.Connections.Any(connection => connection.Id == id);
    
    public void Add(Connection entity)
    {
        var connection = _connectionMapper.ToData(entity);
        _dbContext.Connections.Add(connection);
        _dbContext.SaveChanges();
    }

    public Connection Get(int id)
    {
        var connection = _dbContext.Connections.First(connection => connection.Id == id);
        return _connectionMapper.ToDomain(connection);
    }

    public Connection[] GetRange(int index, int count)
    {
        var connections = _dbContext.Connections.Skip(index).Take(count).ToList();
        return connections.ConvertAll(connection => _connectionMapper.ToDomain(connection)).ToArray();
    }

    public void Update(Connection entity)
    {
        var connection = _connectionMapper.ToData(entity);
        _dbContext.Connections.Update(connection);
        _dbContext.SaveChanges();
    }

    public void Remove(int id)
    {
        _dbContext.Connections.Remove(_dbContext.Connections.First(connection => connection.Id == id));
        _dbContext.SaveChanges();
    }

    public User GetRelatedEntity(Connection source)
    {
        var connection = _connectionMapper.ToData(source);
        var user = _dbContext.Users.First(user => user.Id == connection.UserId);
        return _userMapper.ToDomain(user);
    }
}