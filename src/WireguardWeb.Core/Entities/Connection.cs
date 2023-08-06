using WireguardWeb.Core.Dto.Connection;
using WireguardWeb.Core.Entities.Interfaces;

namespace WireguardWeb.Core.Entities;

public sealed class Connection : IEntity, ITransfer<ConnectionDto>
{
    public int Id { get; }
    public int UserId { get; }
    public string? Info { get; private set; }

    public Connection(
        int id,
        int userId,
        string? info)
    {
        Id = id;
        UserId = userId;
        Info = info;
    }

    public ConnectionDto ToTransfer()
    {
        return new ConnectionDto
        {
            Id = Id,
            UserId = UserId,
            Info = Info
        };
    }

    public void ChangeOf(ConnectionDto dto)
    {
        Info = dto.Info;
    }
}