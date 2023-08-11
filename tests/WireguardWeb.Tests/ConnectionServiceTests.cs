using WireguardWeb.Core.DomainServices;

namespace WireguardWeb.Tests;

public class ConnectionServiceTests
{
    private readonly FakeConnectionRepository _repository;
    private readonly FakeVpnManager _vpnManager;
    private readonly ConnectionService<FakeConnectionRepository, FakeClientConnection> _service;

    public ConnectionServiceTests()
    {
        _repository = new FakeConnectionRepository();
        _vpnManager = new FakeVpnManager();
        _service = new ConnectionService<FakeConnectionRepository, FakeClientConnection>(_repository, _vpnManager);
    }
}