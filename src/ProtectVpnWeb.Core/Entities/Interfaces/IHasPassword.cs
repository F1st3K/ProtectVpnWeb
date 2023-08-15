namespace ProtectVpnWeb.Core.Entities.Interfaces;

public interface IHasPassword
{
    public string HashPassword { get; }
}