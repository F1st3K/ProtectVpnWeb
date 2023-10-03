using ProtectVpnWeb.Core.Entities;
using ProtectVpnWeb.Data.Entities;

namespace ProtectVpnWeb.Contracts.Data.Mappers;

public class UserMapper : IMapper<User, UserEntity>
{
    public User ToDomain(UserEntity entity)
    {
        throw new NotImplementedException();
    }

    public UserEntity ToData(User domain)
    {
        throw new NotImplementedException();
    }
}