using WireguardWeb.Core.Dto.Connection;
using WireguardWeb.Core.Entities.Interfaces;

namespace WireguardWeb.Core.Entities;

public sealed class Connection : IEntity, IHasDto<ConnectionDto>
{
    public int Id { get; }
    public string UserId { get; }

    public Connection(
        int id,
        string userId)
    {
        Id = id;
        UserId = userId;
    }

    public ConnectionDto ToDto()
    {
        return new ConnectionDto
        {
            
        };
    }

    public void ChangeOf(ConnectionDto dto)
    {
        
    }
}