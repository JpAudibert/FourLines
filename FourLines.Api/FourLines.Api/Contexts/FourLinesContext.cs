 using FourLines.Api.DataModels;
using Microsoft.EntityFrameworkCore;

namespace FourLines.Api.Contexts;

public class FourLinesContext(DbContextOptions<FourLinesContext> options) : DbContext(options)
{
    public required IConfiguration Configuration { get; set; }
    public DbSet<Role> Roles { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Facility> Facilities { get; set; } = default!;
    public DbSet<Sport> Sports { get; set; } = default!;
    public DbSet<Court> Courts { get; set; } = default!;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
        .HasOne(user => user.Role)
        .WithMany(roles => roles.Users)
        .HasForeignKey(u => u.RoleId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Facility>()
        .HasOne(facility => facility.Owner)
        .WithMany(user => user.Facilities)
        .HasForeignKey(f => f.OwnerId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Court>()
        .HasOne(court => court.Sport)
        .WithMany(sport => sport.Courts)
        .HasForeignKey(c => c.SportId)
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Court>()
        .HasOne(court => court.Facility)
        .WithMany(facility => facility.Courts)
        .HasForeignKey(c => c.FacilityId)
        .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}
