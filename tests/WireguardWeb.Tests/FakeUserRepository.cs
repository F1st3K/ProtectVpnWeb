using System.Security.Cryptography;
using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Repositories;

namespace WireguardWeb.Tests;

public sealed class FakeUserRepository : IRepository<User>, IUniqueNameRepository<User>
{
    private List<User> _users = new List<User>();

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
        foreach (var user in _users)
        {
            if (user.Id == id)
                return false;
        }
        return true;
    }

    public void Add(User entity)
    {
        _users.Add(entity);
        Count++;
    }

    public User GetById(int id)
    {
        foreach (var user in _users)
        {
            if (user.Id == id)
                return user;
        }

        return null;
    }

    public User[] GetRange(int index, int count)
    {
        return _users.GetRange(index, count).ToArray();
    }

    public void Update(User entity)
    {
        foreach (var user in _users)
        {
            if (user.Id == entity.Id)
            {
                _users.Remove(user);
                _users.Add(entity);
                return;
            }
        }
    }

    public void Remove(int id)
    {
        foreach (var user in _users)
        {
            if (user.Id == id)
            {
                _users.Remove(user);
                return;
            }
        }
    }

    public bool CheckNameUniqueness(string uname)
    {
        foreach (var user in _users)
        {
            if (user.UniqueName == uname)
                return false;
        }

        return true;
    }

    public User GetByUniqueName(string uname)
    {
        foreach (var user in _users)
        {
            if (user.UniqueName == uname)
            {
                return user;
            }
        }

        return null;
    }

    public void Remove(string uname)
    {
        foreach (var user in _users)
        {
            if (user.UniqueName == uname)
            {
                _users.Remove(user);
                return;
            }
        }
    }

    public void FakeInit()
    {
        Add(new User(
            0,
            "user1",
            "pwd1"));
        Add(new User(
            1,
            "user2",
            "pwd2"));
        Add(new User(
            2,
            "user3",
            "pwd3"));
        Add(new User(
            3,
            "user4",
            "pwd4"));
        Add(new User(
            4,
            "user5",
            "pwd5"));
    }
    
    public void Clear()
    {
        _users.Clear();
        Count = 0;
    }
}