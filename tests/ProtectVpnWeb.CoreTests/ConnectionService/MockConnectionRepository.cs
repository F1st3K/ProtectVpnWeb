using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Core.Repositories;

namespace ProtectVpnWeb.CoreTests.ConnectionService;

public sealed class MockConnectionRepository : IRepository<Connection>,
    IOneRelationshipRepository<Connection, User>
{
    private readonly List<Connection> _connections = new();
    private readonly List<User> _users = new();

    public int Count { get; private set; }
    public int GetNextId()
    {
        var next = Count;
        for (; ; next++)
        {
            if (CheckIdUniqueness(next))
                return next;
        }
    }

    public bool CheckIdUniqueness(int id)
    {
        return _connections.All(connection => connection.Id != id);
    }

    public void Add(Connection entity)
    {
        _connections.Add(entity);
        Count++;
    }

    public Connection Get(int id)
    {
        return _connections.FirstOrDefault(connection => connection.Id == id);
    }

    public Connection[] GetRange(int index, int count)
    {
        return _connections.GetRange(index, count).ToArray();
    }

    public void Update(Connection entity)
    {
        foreach (var connection in _connections.Where(connection => connection.Id == entity.Id))
        {
            _connections.Remove(connection);
            _connections.Add(entity);
            return;
        }
    }

    public void Remove(int id)
    {
        foreach (var connection in _connections.Where(connection => connection.Id == id))
        {
            _connections.Remove(connection);
            return;
        }
    }

    public void Clear()
    {
        _connections.Clear();
        _users.Clear();
        Count = 0;
    }
    
    public void FakeInit(IEnumerable<Connection> connections, IEnumerable<User> users)
    {
        Clear();
        foreach (var c in connections) Add(c);
        foreach (var u in users) _users.Add(u);
    }

    public User GetRelatedEntity(Connection source)
    {
        var user = _users.Find(user => user.Id == source.UserId);
        return user;
    }
}