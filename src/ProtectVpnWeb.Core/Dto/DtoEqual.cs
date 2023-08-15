using ProtectVpnWeb.Core.Dto.Connection;
using ProtectVpnWeb.Core.Dto.User;

namespace ProtectVpnWeb.Core.Dto;

public static class DtoEqual
{
    public static bool AreEqual(this UserDto dto, UserDto obj) =>
        dto.UniqueName == obj.UniqueName &&
        dto.Id == obj.Id;
    
    public static bool AreEqual(this UserDto dto, CreateUserDto obj) =>
        dto.UniqueName == obj.UserName;
    
    public static bool AreEqual(this ConnectionDto dto, ConnectionDto obj) =>
        dto.Info == obj.Info &&
        dto.UserId == obj.UserId &&
        dto.Id == obj.Id;
    
    public static bool AreEqual(this ConnectionDto dto, CreateConnectionDto obj) =>
        dto.UserId == obj.UserId;
}