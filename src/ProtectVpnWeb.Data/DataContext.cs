using Microsoft.EntityFrameworkCore;
using ProtectVpnWeb.Data.Entities;

namespace ProtectVpnWeb.Data;

public class DataContext : DbContext
{
    public DbSet<UserEntity> Users { get; private set; }
    public DbSet<ConnectionEntity> Connections { get; private set; }
    public DbSet<string> Tokens { get; private set; }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}