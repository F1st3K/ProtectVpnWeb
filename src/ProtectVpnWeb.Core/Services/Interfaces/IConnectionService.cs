using ProtectVpnWeb.Core.Dto.Connection;

namespace ProtectVpnWeb.Core.Services;

public interface IConnectionService
{
    public void Restart();

    public ConnectionDto GetConnection(int id);

    public ConnectionDto[] GetConnectionsInRange(int startIndex, int count);

    public ConnectionDto CreateConnection(CreateConnectionDto dto);

    public void EditConnection(int id, ConnectionDto dto);

    public void RemoveConnection(int id);
}