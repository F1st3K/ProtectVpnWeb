using WireguardWeb.Core.DomainServices;
using WireguardWeb.Core.Dto;
using WireguardWeb.Core.Dto.User;

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
}