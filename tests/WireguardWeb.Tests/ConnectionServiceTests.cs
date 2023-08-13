using WireguardWeb.Core.DomainServices;
using WireguardWeb.Core.Dto;
using WireguardWeb.Core.Dto.Connection;
using WireguardWeb.Core.Entities;
using WireguardWeb.Core.Exceptions;

namespace WireguardWeb.Tests;

public class ConnectionServiceTests
{
    private readonly FakeConnectionRepository _repository;
    private readonly FakeVpnManager _vpnManager;
    private readonly ConnectionService<FakeConnectionRepository, FakeClientConnection, FakeVpnManager> _service;

    private readonly Connection[] _fakeConnections =
    {
        new(0, 0, "IP=1"),
        new(1, 0, "IP=1"),
        new(2, 2, "IP=3"),
        new(3, 3, "IP=4"),
        new(4, 4, "IP=5"),
        new(5, 0, "IP=6"),
        new(6, 1, "IP=7")
    };

    public ConnectionServiceTests()
    {
        _repository = new FakeConnectionRepository();
        _vpnManager = new FakeVpnManager();
        _service = new ConnectionService<
            FakeConnectionRepository, FakeClientConnection, FakeVpnManager>
            (_repository, _vpnManager);
    }
    
    [Test]
    public async Task Get_Success()
    {
        _repository.FakeInit(_fakeConnections);
        _service.Restart();
        var connectionDto = new ConnectionDto { Id = 0, UserId = 0, Info = "IP=1" };

        var connectionOfId = _service.GetConnection(connectionDto.Id);

        Assert.That(connectionOfId.AreEqual(connectionDto), Is.True);
    }

    [Test]
    public async Task GetRange_Success()
    {
        _repository.FakeInit(_fakeConnections);
        _service.Restart();
        var connections = new ConnectionDto[]
        {
            new() { Id = 3, UserId = 3, Info = "IP=4" },
            new() { Id = 4, UserId = 4, Info = "IP=5" },
        };

        var connectionDtos = _service.GetConnectionsInRange(3, 2);

        Assert.That(connectionDtos, Has.Length.EqualTo(connections.Length));
        for (int i = 0; i < connectionDtos.Length; i++)
            Assert.That(connectionDtos[i].AreEqual(connections[i]), Is.True);
    }

    [Test]
    public async Task Create_Success()
    {
        _repository.FakeInit(_fakeConnections);
        _service.Restart();
        var createConnectionDto = new CreateConnectionDto { UserId = 0 };

        var newConnectionDto = _service.CreateConnection(createConnectionDto);

        Assert.Multiple(() =>
        {
            Assert.That(newConnectionDto.AreEqual(createConnectionDto), Is.True);
            Assert.That(_repository.
                GetById(newConnectionDto.Id).ToTransfer().AreEqual(newConnectionDto), Is.True);
        });
    }

    [Test]
    public async Task Edit_Success()
    {
        _repository.FakeInit(_fakeConnections);
        _service.Restart();
        var connection = new ConnectionDto { Id = 1, UserId = 4, Info = "IP=101" };

        _service.EditConnection(1, connection);

        Assert.That(_service.GetConnection(connection.Id).AreEqual(connection), Is.True);
    }

    [Test]
    public async Task Remove_Success()
    {
        _repository.FakeInit(_fakeConnections);
        _service.Restart();
        var id = 0;

        _service.RemoveConnection(id);

        Assert.Catch<IdNotFoundException>(delegate { _service.GetConnection(id); });
    }
}