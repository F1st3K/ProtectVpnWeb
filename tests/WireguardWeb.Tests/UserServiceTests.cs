using WireguardWeb.Core.DomainServices;

namespace WireguardWeb.Tests;

public sealed class UserServiceTests
{
    private FakeUserRepository _repository;
    private UserService<FakeUserRepository> _service;

    [Fact]
    public async Task Success()
    {
        var repository = new FakeUserRepository();
        var service = new UserService<FakeUserRepository>(repository);
    }
}