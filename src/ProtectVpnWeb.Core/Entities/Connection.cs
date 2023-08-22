using ProtectVpnWeb.Core.Dto.Connection;
using ProtectVpnWeb.Core.Entities.Interfaces;

namespace ProtectVpnWeb.Core.Entities;

public sealed class Connection : IEntity, ITransfer<ConnectionDto>, IHasOneRelationship<User>
{
    public int Id { get; }
    public int UserId { get; private set; }
    public string Info { get; private set; }

    public Connection(
        int id,
        int userId,
        string? info)
    {
        Id = id;
        UserId = userId;
        Info = info ?? string.Empty;
    }

    public ConnectionDto ToTransfer() =>
        new () {
            Id = Id,
            UserId = UserId,
            Info = Info
        };
    

    public void ChangeOf(ConnectionDto dto)
    {
        Info = dto.Info;
        UserId = dto.UserId;
    }
}