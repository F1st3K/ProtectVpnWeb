using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProtectVpnWeb.Data.Entities;

namespace ProtectVpnWeb.Data.ORM;

public class DataContext : DbContext
{
    protected readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql("");
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<ConnectionEntity> Connections { get; set; }
}