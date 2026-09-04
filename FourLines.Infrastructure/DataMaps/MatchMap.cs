namespace FourLines.Infrastructure.DataMaps;

public sealed class MatchMap : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("matches");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ReservationId)
            .IsRequired();

        builder.Property(x => x.SportId)
            .IsRequired();

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(6);

        builder.Property(x => x.Name)
            .HasMaxLength(150);

        builder.HasOne(x => x.Reservation)
            .WithOne()
            .HasForeignKey<Match>(x => x.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Sport)
            .WithMany()
            .HasForeignKey(x => x.SportId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.HasIndex(x => x.ReservationId)
            .IsUnique();
    }
}
