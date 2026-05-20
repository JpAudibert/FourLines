using FourLines.Api.DataModels;
using Microsoft.EntityFrameworkCore;

namespace FourLines.Api.Contexts;

public class FourLinesContext(DbContextOptions<FourLinesContext> options) : DbContext(options)
{
    public required IConfiguration Configuration { get; set; }
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Role> Roles { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string not found"));

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
        .HasOne(user => user.Role)
        .WithMany(roles => roles.Users)
        .HasForeignKey(u => u.RoleId)
        .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}
