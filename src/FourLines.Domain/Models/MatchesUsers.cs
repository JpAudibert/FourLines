namespace FourLines.Domain.Models;

public record MatchesUsers : BaseEntity
{
    public Guid MatchId { get; init; }
    public Guid UserId { get; init; }
    public bool IsFixedPosition { get; init; }
    public int TeamNumber { get; set; }

    public Match Match { get; init; } = default!;
    public User User { get; init; } = default!;
}
