namespace FourLines.Infrastructure.DataMaps;

public class FacilityScheduleMap : IEntityTypeConfiguration<FacilitySchedule>
{
    public void Configure(EntityTypeBuilder<FacilitySchedule> builder)
    {
        builder.ToTable("facility_schedules");

        builder.HasKey(fs => new { fs.FacilityId, fs.DayOfWeek });

        builder.Property(fs => fs.FacilityId)
            .HasColumnName("facility_id")
            .IsRequired();

        builder.Property(fs => fs.DayOfWeek)
            .HasColumnName("day_of_week")
            .HasConversion<int>()
            .IsRequired();

        builder.Property(fs => fs.OpensAt)
            .HasColumnName("opens_at")
            .IsRequired();

        builder.Property(fs => fs.ClosesAt)
            .HasColumnName("closes_at")
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        builder.HasOne(f => f.Facility)
            .WithMany(f => f.Schedules)
            .HasForeignKey(fs => fs.FacilityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
