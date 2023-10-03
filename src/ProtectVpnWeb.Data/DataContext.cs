using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProtectVpnWeb.Data.Entities;

namespace ProtectVpnWeb.Data;

public class DataContext : DbContext
{
    public DbSet<UserEntity> Users { get; private set; }
    public DbSet<ConnectionEntity> Connections { get; private set; }
    public DbSet<string> Tokens { get; private set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        var connectionString = config.GetConnectionString("Database");
        
        optionsBuilder.UseNpgsql(connectionString);
    }
}