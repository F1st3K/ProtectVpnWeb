using Microsoft.EntityFrameworkCore;
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
    private readonly DataContext _dbContext;
    
    public UserRepository(DbContextOptions<DataContext> options)
    {
        _dbContext = new DataContext(options);
    }

    public int Count => _dbContext.Users.Count();

    public int GetNextId() =>
        _dbContext.Users.Max(user => user.Id) + 1;

    public bool CheckIdUniqueness(int id) =>
        !_dbContext.Users.Any(user => user.Id == id);

    public void Add(User entity)
    {
        var user = _userMapper.ToData(entity);
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
    }

    public User Get(int id)
    {
        var user = _dbContext.Users.First(user => user.Id == id);
        return _userMapper.ToDomain(user);
    }

    public User[] GetRange(int index, int count)
    {
        var users = _dbContext.Users.Skip(index).Take(count).ToList();
        return users.ConvertAll(user => _userMapper.ToDomain(user)).ToArray();
    }

    public void Update(User entity)
    {
        var user = _userMapper.ToData(entity);
        _dbContext.Users.Update(user);
        _dbContext.SaveChanges();
    }

    public void Remove(int id)
    {
        _dbContext.Users.Remove(_dbContext.Users.First(user => user.Id == id));
        _dbContext.SaveChanges();
    }

    public bool CheckNameUniqueness(string uname)
    {
        return !_dbContext.Users.Any(user => user.UniqueName == uname);
    }

    public User Get(string uname)
    {
        var user = _dbContext.Users.First(user => user.UniqueName == uname);
        return _userMapper.ToDomain(user);
    }

    public void Remove(string uname)
    {
        _dbContext.Users.Remove(_dbContext.Users.First(user => user.UniqueName == uname));
        _dbContext.SaveChanges();
    }

    public Connection[] GetRelatedEntities(User source)
    {
        var user = _userMapper.ToData(source);
        var connections =
            _dbContext.Connections.Where(connection => connection.UserId == user.Id).ToList();
        return connections.ConvertAll(connection => _connectionMapper.ToDomain(connection)).ToArray();
    }
}