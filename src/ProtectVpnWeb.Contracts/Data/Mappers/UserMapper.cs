using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Data.Entities;

namespace ProtectVpnWeb.Contracts.Data.Mappers;

public class UserMapper : IMapper<User, UserEntity>
{
    public User ToDomain(UserEntity entity)
    {
        return new User(
            entity.Id,
            entity.UniqueName,
            entity.HashPassword,
            Enum.Parse<UserRoles>(entity.Role)
            );
    }

    public UserEntity ToData(User domain)
    {
        return new UserEntity
        {
            Id = domain.Id,
            UniqueName = domain.UniqueName,
            HashPassword = domain.HashPassword,
            Role = domain.Role.ToString()
        };
    }
}