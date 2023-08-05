using WireguardWeb.Core.Dto.User;
using WireguardWeb.Core.Entities.Interfaces;

namespace WireguardWeb.Core.Entities;

public sealed class User : IEntity, IHasDto<UserDto>, IHasUniqueName, IHasPassword
{
    public int Id { get; }

    public string UniqueName { get; }
    
    public string HashPassword { get; }
    
    public User(
        int id,
        string uniqueName,
        string hashPassword)
    {
        Id = id;
        UniqueName = uniqueName;
        HashPassword = hashPassword;
    }

    public UserDto ToDto()
    {
        return new UserDto
        {
            Id = Id,
            UniqueName = UniqueName
        };
    }
}