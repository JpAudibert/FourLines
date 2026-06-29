namespace FourLines.Infrastructure.Contexts;

public class FourLinesContext(DbContextOptions<FourLinesContext> options) : DbContext(options)
{
    public required IConfigurationRoot Configuration;

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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FourLinesContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
