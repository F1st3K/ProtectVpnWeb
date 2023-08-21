using ProtectVpnWeb.Core.Dto;
using ProtectVpnWeb.Core.Dto.User;
using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Core.Exceptions;
using ProtectVpnWeb.Core.Services;
using ProtectVpnWeb.CoreTests.AuthService;

namespace ProtectVpnWeb.CoreTests.UserService;

public sealed class Tests
{
    private readonly MockUserRepository _repository;
    private readonly UserService<MockUserRepository> _service;

    private readonly User[] _fakeUsers =
    {
        new(0, "user1", "pwd1"),
        new(1, "user2", "pwd2"),
        new(2, "user3", "pwd3"),
        new(3, "user4", "pwd4"),
        new(4, "user5", "pwd5")
    };

    public Tests()
    {
        _repository = new MockUserRepository();
        _service = new UserService<MockUserRepository>(_repository);
    }

    private bool CheckInRepository(UserDto dto) =>
        _repository.GetById(dto.Id).ToTransfer().AreEqual(dto);
    
    [Test]
    public void Get_Success()
    {
        _repository.FakeInit(_fakeUsers);
        var user = new UserDto { Id = 2, UniqueName = "user3" };

        var userOfId = _service.GetUser(user.Id);
        var userOfUname = _service.GetUser(user.UniqueName);

        Assert.Multiple(() =>
        {
            Assert.That(userOfUname.AreEqual(user), Is.True);
            Assert.That(userOfId.AreEqual(userOfUname), Is.True);
            Assert.That(CheckInRepository(userOfId), Is.True);
        });
    }

    [Test]
    public void GetRange_Success()
    {
        _repository.FakeInit(_fakeUsers);
        var users = new UserDto[]
        {
            new() { Id = 3, UniqueName = "user4" },
            new() { Id = 4, UniqueName = "user5" }
        };

        var userDtos = _service.GetUsersInRange(3, 2);

        Assert.That(userDtos, Has.Length.EqualTo(users.Length));
        for (int i = 0; i < userDtos.Length; i++)
            Assert.Multiple(() =>
            {
                Assert.That(userDtos[i].AreEqual(users[i]), Is.True);
                Assert.That(CheckInRepository(userDtos[i]), Is.True);
            });
    }

    [Test]
    public void Edit_Success()
    {
        _repository.FakeInit(_fakeUsers);
        var userOfId = new UserDto 
            { Id = 0, UniqueName = "editUser1", Role = UserRoles.User.ToString()};
        var userOfUname = new UserDto 
            { Id = 1, UniqueName = "editUser2", Role = UserRoles.User.ToString()};
        var expected = new UserDto 
            { Id = 0, UniqueName = "user2", Role = UserRoles.User.ToString()};

        _service.EditUser(expected.Id, userOfId);
        _service.EditUser(expected.UniqueName, userOfUname);

        Assert.Multiple(() =>
        {
            Assert.That(_service.GetUser(userOfId.Id).AreEqual(userOfId), Is.True);
            Assert.That(_service.GetUser(userOfUname.Id).AreEqual(userOfUname), Is.True);
            Assert.That(CheckInRepository(userOfUname), Is.True);
        });
    }

    [Test]
    public void Create_Success()
    {
        _repository.FakeInit(_fakeUsers);
        var newUser = new CreateUserDto
            { UniqueName = "user6", Role = UserRoles.Admin.ToString(), Password = "pwd6" };
        var hasher = new MockHashService("//hash");

        _service.CreateUser(newUser, hasher);

        var user = _repository.GetByUniqueName(newUser.UniqueName);
        Assert.Multiple(() =>
        {
            Assert.That(user.HashPassword, Is.EqualTo(hasher.GetHash(newUser.Password)));
            Assert.That(user.Role, Is.EqualTo(UserRoles.Admin));
        });
    }
    
    [Test]
    public void Remove_Success()
    {
        _repository.FakeInit(_fakeUsers);
        const int id = 3;
        const string uname = "user5";

        _service.RemoveUser(id);
        _service.RemoveUser(uname);
        
        Assert.Multiple(() =>
        {
            Assert.That(_repository.CheckIdUniqueness(id), Is.True);
            Assert.That(_repository.CheckNameUniqueness(uname), Is.True);
        });
    }

    [Test]
    public void Get_Exception()
    {
        _repository.FakeInit(_fakeUsers);

        Assert.Catch<InvalidArgumentException>(delegate { _service.GetUser(-1); });
        Assert.Catch<InvalidArgumentException>(delegate { _service.GetUser(string.Empty); });

        var id = _repository.Count;
        while (_repository.CheckIdUniqueness(id) == false) id++;
        Assert.Catch<NotFoundException>(delegate { _service.GetUser(id); });

        var uname = new Guid().ToString();
        while (_repository.CheckNameUniqueness(uname) == false) uname = new Guid().ToString();
        Assert.Catch<NotFoundException>(delegate { _service.GetUser(uname); });
    }

    [Test]
    public void GetRange_Exception()
    {
        _repository.FakeInit(_fakeUsers);

        Assert.Catch<InvalidArgumentException>(delegate
            { _service.GetUsersInRange(-1, 1); });
        Assert.Catch<InvalidArgumentException>(delegate
            { _service.GetUsersInRange(0, 0); });

        var startIndex = _repository.Count / 2;
        var count = _repository.Count - startIndex + 1;
        Assert.Catch<RangeException>(delegate
            { _service.GetUsersInRange(startIndex, count); });
    }

    [Test]
    public void Edit_Exception()
    {
        _repository.FakeInit(_fakeUsers);

        var dto = new UserDto
        {
            Id = 0,
            UniqueName = "user1",
            Role = UserRoles.User.ToString()
        };

        Assert.Catch<InvalidArgumentException>(delegate
        {
            _service.EditUser(-1, dto);
        });
        Assert.Catch<InvalidArgumentException>(delegate
        {
            _service.EditUser(string.Empty, dto);
        });


        var id = _repository.Count;
        while (_repository.CheckIdUniqueness(id) == false) id++;
        Assert.Catch<NotFoundException>(delegate { _service.EditUser(id, dto); });

        var uname = new Guid().ToString();
        while (_repository.CheckNameUniqueness(uname) == false) uname = new Guid().ToString();
        Assert.Catch<NotFoundException>(delegate { _service.EditUser(uname, dto); });

        Assert.Catch<NonIdenticalException>(delegate { _service.EditUser(1, dto); });
        Assert.Catch<NonIdenticalException>(delegate { _service.EditUser("user2", dto); });
    }

    [Test]
    public void Create_Exception()
    {
        _repository.FakeInit(_fakeUsers);

        Assert.Catch<InvalidArgumentException>(delegate
        { _service.CreateUser(
            new CreateUserDto
                { UniqueName = string.Empty, Role = UserRoles.User.ToString(), Password = "pwd" },
            new MockHashService(string.Empty)); });

        Assert.Catch<InvalidArgumentException>(delegate
        { _service.CreateUser(new CreateUserDto
            { UniqueName = "user7", Role = UserRoles.User.ToString(), Password = string.Empty },
            new MockHashService(string.Empty)); });

        Assert.Catch<InvalidArgumentException>(delegate
        { _service.CreateUser(
            new CreateUserDto
                { UniqueName = "user7", Role = "user", Password = "pwd" },
            new MockHashService(string.Empty)); });
    }

    [Test]
    public void Remove_Exception()
    {
        _repository.FakeInit(_fakeUsers);

        Assert.Catch<InvalidArgumentException>(delegate { _service.RemoveUser(-1); });
        Assert.Catch<InvalidArgumentException>(delegate 
            { _service.RemoveUser(string.Empty); });
        
        var id = _repository.Count;
        while (_repository.CheckIdUniqueness(id) == false) id++;
        Assert.Catch<NotFoundException>(delegate { _service.RemoveUser(id); });
        
        var uname = new Guid().ToString();
        while (_repository.CheckNameUniqueness(uname) == false) uname = new Guid().ToString();
        Assert.Catch<NotFoundException>(delegate 
            { _service.RemoveUser(uname); });
    }
}