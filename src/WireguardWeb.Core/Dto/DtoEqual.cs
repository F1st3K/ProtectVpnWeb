using WireguardWeb.Core.Dto.User;

namespace WireguardWeb.Core.Dto;

public static class DtoEqual
{
    public static bool AreEqual(this UserDto dto, UserDto obj) =>
        dto.UniqueName == obj.UniqueName &&
        dto.Id == obj.Id;
}