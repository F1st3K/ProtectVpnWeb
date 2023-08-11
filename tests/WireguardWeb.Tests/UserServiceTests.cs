using WireguardWeb.Core.DomainServices;
using WireguardWeb.Core.Dto;
using WireguardWeb.Core.Dto.User;
using WireguardWeb.Core.Exceptions;

namespace WireguardWeb.Tests;

public sealed class UserServiceTests
{
    private readonly FakeUserRepository _repository;
    private readonly UserService<FakeUserRepository> _service;

    public UserServiceTests()
    {
        _repository = new FakeUserRepository();
        _service = new UserService<FakeUserRepository>(_repository);
    }

    [Test]
    public async Task Get_Success()
    {
        _repository.Clear();
        _repository.FakeInit();
        var user = new UserDto { Id = 0, UniqueName = "user1" };

        var userOfId = _service.GetUser(user.Id);
        var userOfUname = _service.GetUser(user.UniqueName);
        
        Assert.Multiple(() =>
        {
            Assert.That(userOfUname.AreEqual(user), Is.True);
            Assert.That(userOfId.AreEqual(userOfUname), Is.True);
        });
    }

    [Test]
    public async Task GetRange_Success()
    {
        _repository.Clear();
        _repository.FakeInit();
        var users = new UserDto[]
        {
            new UserDto { Id = 3, UniqueName = "user4" },
            new UserDto { Id = 4, UniqueName = "user5" }
        };

        var userDtos = _service.GetUsersInRange(3, 2);
        
        for (int i = 0; i < userDtos.Length; i++)
            Assert.That(userDtos[i].AreEqual(users[i]), Is.True);
    }

    [Test]
    public async Task Edit_Success()
    {
        _repository.Clear();
        _repository.FakeInit();
        var userOfId = new UserDto { Id = 0, UniqueName = "editUser1" };
        var userOfUname = new UserDto { Id = 1, UniqueName = "editUser2" };
        var expected = new UserDto { Id = 0, UniqueName = "user2" };
        
        _service.EditUser(expected.Id, userOfId);
        _service.EditUser(expected.UniqueName, userOfUname);
        
        Assert.Multiple(() =>
        {
            Assert.That(_service.GetUser(userOfId.Id).AreEqual(userOfId), Is.True);
            Assert.That(_service.GetUser(userOfUname.Id).AreEqual(userOfUname), Is.True);
        });
    }

    [Test]
    public async Task Get_Exception()
    {
        _repository.Clear();
        _repository.FakeInit();

        Assert.Catch<InvalidArgumentException>(delegate { _service.GetUser(-1); });
        Assert.Catch<InvalidArgumentException>(delegate { _service.GetUser(string.Empty); });

        var id = _repository.Count;
        while (_repository.CheckIdUniqueness(id) == false) id++;
        Assert.Catch<IdNotFoundException>(delegate { _service.GetUser(id); });

        var uname = new Guid().ToString();
        while (_repository.CheckNameUniqueness(uname) == false) uname = new Guid().ToString();
        Assert.Catch<UserNameNotFoundException>(delegate { _service.GetUser(uname); });
    }

    [Test]
    public async Task GetRange_Exception()
    {
        _repository.Clear();
        _repository.FakeInit();
        
        Assert.Catch<InvalidArgumentException>(delegate { _service.GetUsersInRange(-1, 1); });
        Assert.Catch<InvalidArgumentException>(delegate { _service.GetUsersInRange(0, 0); });

        var startIndex = _repository.Count / 2;
        var count = _repository.Count - startIndex + 1;
        Assert.Catch<RangeException>(delegate { _service.GetUsersInRange(startIndex, count); });
    }

    [Test]
    public async Task Edit_Exception()
    {
        _repository.Clear();
        _repository.FakeInit();

        var dto = new UserDto
        {
            Id = 0,
            UniqueName = "user1"
        };
        
        Assert.Catch<InvalidArgumentException>(delegate { _service.EditUser(-1, dto); });
        Assert.Catch<InvalidArgumentException>(delegate { _service.EditUser(string.Empty, dto); });
        
        
        var id = _repository.Count;
        while (_repository.CheckIdUniqueness(id) == false) id++;
        Assert.Catch<IdNotFoundException>(delegate { _service.EditUser(id, dto); });

        var uname = new Guid().ToString();
        while (_repository.CheckNameUniqueness(uname) == false) uname = new Guid().ToString();
        Assert.Catch<UserNameNotFoundException>(delegate { _service.EditUser(uname, dto); });

        Assert.Catch<NonIdenticalException>(delegate { _service.EditUser(1, dto); });
        Assert.Catch<NonIdenticalException>(delegate { _service.EditUser("user2", dto); });
    }
}