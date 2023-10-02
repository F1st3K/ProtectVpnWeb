namespace ProtectVpnWeb.Data.Entities;

public sealed class UserEntity : BaseEntity
{
    public string UniqueName { get; set; }
    public string HashPassword { get; set; }
    public string Role { get; set; }
}