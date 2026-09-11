namespace FourLines.Domain.Models;

public record MatchesUsers : BaseEntity
{
    public Guid MatchId { get; init; }
    public Guid UserId { get; init; }
    public bool IsGoalKeeper { get; init; }

    public Match Match { get; init; } = default!;
    public User User { get; init; } = default!;

}
