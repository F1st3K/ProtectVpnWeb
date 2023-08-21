using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Core.Entities.Interfaces;
using ProtectVpnWeb.Core.Repositories;

namespace ProtectVpnWeb.CoreTests.UserService;

public sealed class MockUserRepository : IRepository<User>, IUniqueNameRepository<User>,
    IManyRelationshipsRepository<User, Connection>
{
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
        return _users.All(user => user.Id != id);
    }

    public void Add(User entity)
    {
        _users.Add(entity);
        Count++;
    }

    public User GetById(int id)
    {
        return _users.FirstOrDefault(user => user.Id == id);
    }

    public User[] GetRange(int index, int count)
    {
        return _users.GetRange(index, count).ToArray();
    }

    public void Update(User entity)
    {
        foreach (var user in _users.Where(user => user.Id == entity.Id))
        {
            _users.Remove(user);
            _users.Add(entity);
            return;
        }
    }

    public void Remove(int id)
    {
        foreach (var user in _users.Where(user => user.Id == id))
        {
            _users.Remove(user);
            return;
        }
    }

    public bool CheckNameUniqueness(string uname)
    {
        return _users.All(user => user.UniqueName != uname);
    }

    public User GetByUniqueName(string uname)
    {
        return _users.FirstOrDefault(user => user.UniqueName == uname);
    }

    public void Remove(string uname)
    {
        foreach (var user in _users.Where(user => user.UniqueName == uname))
        {
            _users.Remove(user);
            return;
        }
    }

    public void FakeInit(IEnumerable<User> users)
    {
        Clear();
        foreach (var u in users)
            Add(u);
    }
    
    public void Clear()
    {
        _users.Clear();
        Count = 0;
    }

    public Connection[] GetRelatedEntities(User source)
    {
        throw new NotImplementedException();
    }
}