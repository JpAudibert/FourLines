namespace FourLines.Infrastructure.DataMaps;

public class CourtMap : IEntityTypeConfiguration<Court>
{
    public void Configure(EntityTypeBuilder<Court> builder)
    {
        builder.ToTable("courts");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.FacilityId).IsRequired();

        builder.Property(c => c.SportId).IsRequired();

        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);

        builder.Property(c => c.IsActive).IsRequired();

        builder.ComplexProperty(
            c => c.DefaultPrice,
            price =>
            {
                price
                    .Property(p => p.Amount)
                    .HasColumnName("amount")
                    .HasPrecision(19, 4)
                    .IsRequired();

                price
                    .Property(p => p.Currency)
                    .HasColumnName("currency")
                    .HasMaxLength(3)
                    .IsRequired();
            }
        );

        builder.Property(c => c.RentingPeriodInMinutes).IsRequired();
        
        builder.Property(c => c.MaintenancePeriodInMinutes).IsRequired().HasDefaultValue(0);

        builder.Property(c => c.CreatedAt).IsRequired();

        builder.Property(c => c.UpdatedAt).IsRequired();

        builder
            .HasOne(c => c.Sport)
            .WithMany(s => s.Courts)
            .HasForeignKey(c => c.SportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(c => c.Facility)
            .WithMany(f => f.Courts)
            .HasForeignKey(c => c.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
