using ProtectVpnWeb.Core.Dto.User;
using ProtectVpnWeb.Core.Entities.Interfaces;

namespace ProtectVpnWeb.Core.Entities;

public sealed class User : IEntity, ITransfer<UserDto>, IHasUniqueName
{
    public int Id { get; }

    public string UniqueName { get; private set; }
    
    public string HashPassword { get; private set; }
    
    public UserRoles Role { get; private set; }
    
    public User(
        int id,
        string uniqueName,
        string hashPassword,
        UserRoles? role)
    {
        Id = id;
        UniqueName = uniqueName;
        HashPassword = hashPassword;
        Role = role ?? UserRoles.User;
    }

    public UserDto ToTransfer()
    {
        return new UserDto
        {
            Id = Id,
            UniqueName = UniqueName,
            Role = Role.ToString()
        };
    }

    public void ChangeOf(UserDto dto)
    {
        UniqueName = dto.UniqueName;
        if (Enum.TryParse(dto.Role, out UserRoles role))
            Role = role;
    }
}