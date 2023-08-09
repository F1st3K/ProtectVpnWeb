using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Repositories;

namespace WireguardWeb.Tests;

public sealed class FakeConnectionRepository : IRepository<Connection>
{
    private List<Connection> _connections;

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
        foreach (var connection in _connections)
        {
            if (connection.Id == id)
                return false;
        }
        return true;
    }

    public void Add(Connection entity)
    {
        _connections.Add(entity);
        Count++;
    }

    public Connection GetById(int id)
    {
        foreach (var connection in _connections)
        {
            if (connection.Id == id)
                return connection;
        }

        return null;
    }

    public Connection[] GetRange(int index, int count)
    {
        return _connections.GetRange(index, count).ToArray();
    }

    public void Update(Connection entity)
    {
        foreach (var connection in _connections)
        {
            if (connection.Id == entity.Id)
            {
                _connections.Remove(connection);
                _connections.Add(entity);
                return;
            }
        }
    }

    public void Remove(int id)
    {
        foreach (var connection in _connections)
        {
            if (connection.Id == id)
            {
                _connections.Remove(connection);
                return;
            }
        }
    }
}