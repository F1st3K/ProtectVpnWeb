using ProtectVpnWeb.Core.Dto;
using ProtectVpnWeb.Core.Dto.Connection;
using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Core.Exceptions;
using ProtectVpnWeb.Core.Services;

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
        _repository = new MockConnectionRepository();
        _vpnService = new MockVpnService();
        _service = new ConnectionService<
            MockConnectionRepository, MockClientConnection, MockVpnService>
            (_repository, _vpnService);
    }

    private bool CheckInRepository(ConnectionDto dto) =>
        _repository.Get(dto.Id).ToTransfer().AreEqual(dto);
    
    private bool CheckInVpnManger(ConnectionDto dto) =>
        _vpnService.GetById(dto.Id).ToTransfer().ToTransfer().AreEqual(dto);
    
    [Test]
    public void Get_Success()
    {
        _repository.FakeInit(_fakeConnections, _fakeUsers);
        _service.Restart();
        var connectionDto = _fakeConnections[0].ToTransfer();

        var connectionOfId = _service.GetConnection(connectionDto.Id);
        Assert.Multiple(() =>
        {
            Assert.That(connectionOfId.AreEqual(connectionDto), Is.True);
            Assert.That(CheckInRepository(connectionOfId), Is.True);
            Assert.That(CheckInVpnManger(connectionOfId), Is.True);
        });
    }

    [Test]
    public void GetRange_Success()
    {
        _repository.FakeInit(_fakeConnections, _fakeUsers);
        
        _service.Restart();
        var connections = new ConnectionDto[]
        {
            _fakeConnections[3].ToTransfer(),
            _fakeConnections[4].ToTransfer(),
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
    public void Create_Success()
    {
        _repository.FakeInit(_fakeConnections, _fakeUsers);
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
    public void Edit_Success()
    {
        _repository.FakeInit(_fakeConnections, _fakeUsers);
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
    public void Remove_Success()
    {
        _repository.FakeInit(_fakeConnections, _fakeUsers);
        _service.Restart();
        var id = 6;
        
        _service.RemoveConnection(id);
        
        Assert.Multiple(() =>
        {
            Assert.Catch<NotFoundException>(delegate { _service.GetConnection(id); });
            Assert.That(_repository.Get(id), Is.Null);
            Assert.That(_vpnService.GetById(id), Is.Null);
        });
    }

    [Test]
    public void GetUserByConnection_Success()
    {
        _repository.FakeInit(_fakeConnections, _fakeUsers);
        _service.Restart();
        var id = 5;
        var fakeUser = _fakeUsers[0];

        var user = _service.GetUserByConnection(5);
        
        Assert.That(user.AreEqual(fakeUser.ToTransfer()), Is.True);
    }

    [Test]
    public void Get_Exception()
    {
        _repository.FakeInit(_fakeConnections, _fakeUsers);
        _service.Restart();

        Assert.Catch<InvalidArgumentException>(delegate { _service.GetConnection(-1); });
        
        var id = _repository.Count;
        while (_repository.CheckIdUniqueness(id) == false) id++;
        Assert.Catch<NotFoundException>(delegate { _service.GetConnection(id); });
    }

    [Test]
    public void GetRange_Exception()
    {
        _repository.FakeInit(_fakeConnections, _fakeUsers);
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
    public void Create_Exception()
    {
        _repository.FakeInit(_fakeConnections, _fakeUsers);
        _service.Restart();
        
        Assert.Catch<InvalidArgumentException>(delegate 
            { _service.CreateConnection(new CreateConnectionDto { UserId = -1 }); });

        _vpnService.StopServer();
        Assert.Catch<NotRunningException>(delegate
            { _service.CreateConnection(new CreateConnectionDto { UserId = 0 }); });
    }

    [Test]
    public void Edit_Exception()
    {
        _repository.FakeInit(_fakeConnections, _fakeUsers);
        _service.Restart();
        var dto = _fakeConnections[0].ToTransfer();
        
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
    public void Remove_Exception()
    {
        _repository.FakeInit(_fakeConnections, _fakeUsers);
        _service.Restart();

        Assert.Catch<InvalidArgumentException>(delegate 
            { _service.RemoveConnection(-1); });
        
        var id = _repository.Count;
        while (_repository.CheckIdUniqueness(id) == false) id++;
        Assert.Catch<NotFoundException>(delegate 
            { _service.RemoveConnection(id); });
    }
    
    [Test]
    public void GetUserByConnection_Exception()
    {
        _repository.FakeInit(_fakeConnections, _fakeUsers);
        _service.Restart();
        
        Assert.Catch<InvalidArgumentException>(delegate 
            { _service.GetUserByConnection(-1); });
        
        var id = _repository.Count;
        while (_repository.CheckIdUniqueness(id) == false) id++;
        Assert.Catch<NotFoundException>(delegate 
            { _service.GetUserByConnection(id); });
    }
}