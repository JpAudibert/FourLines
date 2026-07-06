namespace FourLines.Infrastructure.DataMaps;

public sealed class ReservationMap : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("reservations");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("id");

        builder.Property(r => r.CourtId)
            .HasColumnName("court_id")
            .IsRequired();

        builder.Property(r => r.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(r => r.Status)
            .HasColumnName("status")
            .HasConversion<int>()
            .IsRequired();

        builder.ComplexProperty(r => r.Period, period =>
        {
            period.Property(p => p.Start)
                .HasColumnName("starts_at")
                .IsRequired();

            period.Property(p => p.End)
                .HasColumnName("ends_at")
                .IsRequired();
        });

        builder.HasOne<Court>()
            .WithMany(c => c.Reservations)
            .HasForeignKey(r => r.CourtId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
