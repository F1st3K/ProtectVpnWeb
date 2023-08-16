using ProtectVpnWeb.Core.Dto;
using ProtectVpnWeb.Core.Dto.User;
using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Core.Exceptions;
using ProtectVpnWeb.Core.Services.Implementations;

namespace ProtectVpnWeb.Tests.UserService;

public sealed class Tests
{
    private readonly MockUserRepository _repository;
    private readonly UserService<MockUserRepository> _service;

    private readonly User[] _fakeUsers =
    {
        new(0, "user1", "pwd1", null),
        new(1, "user2", "pwd2", null),
        new(2, "user3", "pwd3", null),
        new(3, "user4", "pwd4", null),
        new(4, "user5", "pwd5", null)
    };

    public Tests()
    {
        _repository = new MockUserRepository();
        _service = new UserService<MockUserRepository>(_repository);
    }

    private bool CheckInRepository(UserDto dto) =>
        _repository.GetById(dto.Id).ToTransfer().AreEqual(dto);
    
    [Test]
    public async Task Get_Success()
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
    public async Task GetRange_Success()
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
    public async Task Edit_Success()
    {
        _repository.FakeInit(_fakeUsers);
        var userOfId = new UserDto { Id = 0, UniqueName = "editUser1" };
        var userOfUname = new UserDto { Id = 1, UniqueName = "editUser2" };
        var expected = new UserDto { Id = 0, UniqueName = "user2" };

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
    public async Task Get_Exception()
    {
        _repository.FakeInit(_fakeUsers);

        Assert.Catch<InvalidArgumentException>(delegate { _service.GetUser(-1); });
        Assert.Catch<InvalidArgumentException>(delegate { _service.GetUser(string.Empty); });

        var id = _repository.Count;
        while (_repository.CheckIdUniqueness(id) == false) id++;
        Assert.Catch<IdNotFoundException>(delegate { _service.GetUser(id); });

        var uname = new Guid().ToString();
        while (_repository.CheckNameUniqueness(uname) == false) uname = new Guid().ToString();
        Assert.Catch<UniqNameNotFoundException>(delegate { _service.GetUser(uname); });
    }

    [Test]
    public async Task GetRange_Exception()
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
    public async Task Edit_Exception()
    {
        _repository.FakeInit(_fakeUsers);

        var dto = new UserDto
        {
            Id = 0,
            UniqueName = "user1"
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
        Assert.Catch<IdNotFoundException>(delegate { _service.EditUser(id, dto); });

        var uname = new Guid().ToString();
        while (_repository.CheckNameUniqueness(uname) == false) uname = new Guid().ToString();
        Assert.Catch<UniqNameNotFoundException>(delegate { _service.EditUser(uname, dto); });

        Assert.Catch<NonIdenticalException>(delegate { _service.EditUser(1, dto); });
        Assert.Catch<NonIdenticalException>(delegate { _service.EditUser("user2", dto); });
    }
}