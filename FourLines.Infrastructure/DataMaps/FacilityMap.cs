namespace FourLines.Infrastructure.DataMaps;

public class FacilityMap : IEntityTypeConfiguration<Facility>
{
    public void Configure(EntityTypeBuilder<Facility> builder)
    {
        builder.ToTable("facilities");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.OwnerId)
            .IsRequired();

        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.Address)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.State)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.ZipCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(f => f.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(f => f.RegistrationNumber)
            .IsUnique();

        builder.Property(f => f.CreatedAt)
            .IsRequired();

        builder.Property(f => f.UpdatedAt)
            .IsRequired();

        builder.HasOne(f => f.Owner)
            .WithMany(u => u.Facilities)
            .HasForeignKey(f => f.OwnerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(f => f.Courts)
            .WithOne(c => c.Facility)
            .HasForeignKey(c => c.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(f => f.Schedules)
            .WithOne(s => s.Facility)
            .HasForeignKey(s => s.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
