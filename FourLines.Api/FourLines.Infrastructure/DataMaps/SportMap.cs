using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FourLines.Infrastructure.DataMaps;

public class SportMap : IEntityTypeConfiguration<Sport>
{
    public void Configure(EntityTypeBuilder<Sport> builder)
    {
        builder.ToTable("sports");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Indoor)
            .IsRequired();

        builder.Property(s => s.StartingPlayersCount)
            .IsRequired();

        builder.Property(s => s.MaxPlayersCount)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .IsRequired();

        builder.HasMany(s => s.Courts)
            .WithOne(c => c.Sport)
            .HasForeignKey(c => c.SportId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
