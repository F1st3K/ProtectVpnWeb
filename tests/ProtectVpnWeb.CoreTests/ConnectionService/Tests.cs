using ProtectVpnWeb.Core.Dto;
using ProtectVpnWeb.Core.Dto.Connection;
using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Core.Exceptions;
using ProtectVpnWeb.Core.Services.Implementations;

namespace ProtectVpnWeb.CoreTests.ConnectionService;

public class Tests
{
    private readonly MockConnectionRepository _repository;
    private readonly MockVpnService _vpnService;
    private readonly ConnectionService<
        MockConnectionRepository, MockClientConnection, MockVpnService> _service;

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

    public Tests()
    {
        _repository = new MockConnectionRepository();
        _vpnService = new MockVpnService();
        _service = new ConnectionService<
            MockConnectionRepository, MockClientConnection, MockVpnService>
            (_repository, _vpnService);
    }

    private bool CheckInRepository(ConnectionDto dto) =>
        _repository.GetById(dto.Id).ToTransfer().AreEqual(dto);
    
    private bool CheckInVpnManger(ConnectionDto dto) =>
        _vpnService.GetById(dto.Id).ToTransfer().ToTransfer().AreEqual(dto);
    
    [Test]
    public async Task Get_Success()
    {
        _repository.FakeInit(_fakeConnections);
        _service.Restart();
        var connectionDto = new ConnectionDto { Id = 0, UserId = 0, Info = "IP=1" };

        var connectionOfId = _service.GetConnection(connectionDto.Id);
        Assert.Multiple(() =>
        {
            Assert.That(connectionOfId.AreEqual(connectionDto), Is.True);
            Assert.That(CheckInRepository(connectionOfId), Is.True);
            Assert.That(CheckInVpnManger(connectionOfId), Is.True);
        });
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
            Assert.Multiple(() =>
            {
                Assert.That(connectionDtos[i].AreEqual(connections[i]), Is.True);
                Assert.That(CheckInRepository(connectionDtos[i]), Is.True);
                Assert.That(CheckInVpnManger(connectionDtos[i]), Is.True);
            });
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
            Assert.That(CheckInRepository(newConnectionDto), Is.True);
            Assert.That(CheckInVpnManger(newConnectionDto), Is.True);
        });
    }

    [Test]
    public async Task Edit_Success()
    {
        _repository.FakeInit(_fakeConnections);
        _service.Restart();
        var connection = new ConnectionDto { Id = 1, UserId = 4, Info = "IP=101" };

        _service.EditConnection(1, connection);
        var editConnectionDto = _service.GetConnection(connection.Id);
        
        Assert.Multiple(() =>
        {
            Assert.That(editConnectionDto.AreEqual(connection), Is.True);
            Assert.That(CheckInRepository(editConnectionDto), Is.True);
            Assert.That(CheckInVpnManger(editConnectionDto), Is.True);
        });
    }

    [Test]
    public async Task Remove_Success()
    {
        _repository.FakeInit(_fakeConnections);
        _service.Restart();
        var id = 6;
        
        _service.RemoveConnection(id);
        
        Assert.Multiple(() =>
        {
            Assert.Catch<NotFoundException>(delegate { _service.GetConnection(id); });
            Assert.That(_repository.GetById(id), Is.Null);
            Assert.That(_vpnService.GetById(id), Is.Null);
        });
    }

    [Test]
    public async Task Get_Exception()
    {
        _repository.FakeInit(_fakeConnections);
        _service.Restart();

        Assert.Catch<InvalidArgumentException>(delegate { _service.GetConnection(-1); });
        
        var id = _repository.Count;
        while (_repository.CheckIdUniqueness(id) == false) id++;
        Assert.Catch<NotFoundException>(delegate { _service.GetConnection(id); });
    }

    [Test]
    public async Task GetRange_Exception()
    {
        _repository.FakeInit(_fakeConnections);
        _service.Restart();

        Assert.Catch<InvalidArgumentException>(delegate
            { _service.GetConnectionsInRange(-1, 1); });
        Assert.Catch<InvalidArgumentException>(delegate
            { _service.GetConnectionsInRange(0, 0); });
        
        var startIndex = _repository.Count / 2;
        var count = _repository.Count - startIndex + 1;
        Assert.Catch<RangeException>(delegate 
            { _service.GetConnectionsInRange(startIndex, count); });
    }

    [Test]
    public async Task Create_Exception()
    {
        _repository.FakeInit(_fakeConnections);
        _service.Restart();
        
        Assert.Catch<InvalidArgumentException>(delegate 
            { _service.CreateConnection(new CreateConnectionDto { UserId = -1 }); });

        _vpnService.StopServer();
        Assert.Catch<NotRunningException>(delegate
            { _service.CreateConnection(new CreateConnectionDto { UserId = 0 }); });
    }

    [Test]
    public async Task Edit_Exception()
    {
        _repository.FakeInit(_fakeConnections);
        _service.Restart();
        var dto = new ConnectionDto
        {
            Id = 0,
            UserId = 0,
            Info = "IP=1"
        };
        
        Assert.Catch<InvalidArgumentException>(delegate 
            { _service.EditConnection(-1, dto); });
        
        var id = _repository.Count;
        while (_repository.CheckIdUniqueness(id) == false) id++;
        Assert.Catch<NotFoundException>(delegate
            { _service.EditConnection(id, dto); });

        Assert.Catch<NonIdenticalException>(delegate 
            { _service.EditConnection(1, dto); });
    }

    [Test]
    public async Task Remove_Exception()
    {
        _repository.FakeInit(_fakeConnections);
        _service.Restart();

        Assert.Catch<InvalidArgumentException>(delegate 
            { _service.RemoveConnection(-1); });
        
        var id = _repository.Count;
        while (_repository.CheckIdUniqueness(id) == false) id++;
        Assert.Catch<NotFoundException>(delegate 
            { _service.RemoveConnection(id); });
    }
}