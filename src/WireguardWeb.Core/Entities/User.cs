using WireguardWeb.Core.Dto.User;
using WireguardWeb.Core.Entities.Interfaces;

namespace WireguardWeb.Core.Entities;

public sealed class User : IEntity, ITransfer<UserDto>, IHasUniqueName, IHasPassword
{
    public int Id { get; }

    public string UniqueName { get; private set; }
    
    public string HashPassword { get; private set; }
    
    public User(
        int id,
        string uniqueName,
        string hashPassword)
    {
        Id = id;
        UniqueName = uniqueName;
        HashPassword = hashPassword;
    }

    public UserDto ToTransfer()
    {
        return new UserDto
        {
            Id = Id,
            UniqueName = UniqueName
        };
    }

    public void ChangeOf(UserDto dto)
    {
        UniqueName = dto.UniqueName;
    }
}