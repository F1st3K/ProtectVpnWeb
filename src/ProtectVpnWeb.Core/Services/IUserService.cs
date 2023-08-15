using ProtectVpnWeb.Core.Dto.User;

namespace ProtectVpnWeb.Core.Services;

public interface IUserService
{
    public UserDto GetUser(string userName);

    public UserDto[] GetUsersInRange(int startIndex, int count);

    public void EditUser(string userName, UserDto dto);

    public void EditUser(int id, UserDto dto);
}