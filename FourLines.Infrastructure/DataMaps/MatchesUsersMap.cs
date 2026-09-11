namespace FourLines.Infrastructure.DataMaps;

public sealed class MatchesUsersMap : IEntityTypeConfiguration<MatchesUsers>
{
    public void Configure(EntityTypeBuilder<MatchesUsers> builder)
    {
        builder.ToTable("matches_users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MatchId)
            .IsRequired();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.IsGoalKeeper)
            .IsRequired();

        builder.HasOne(x => x.Match)
            .WithMany(x => x.MatchesUsers)
            .HasForeignKey(x => x.MatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(x => x.MatchesUsers)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // A user can only participate once in the same match.
        builder.HasIndex(x => new
        {
            x.MatchId,
            x.UserId
        })
        .IsUnique();
    }
}
