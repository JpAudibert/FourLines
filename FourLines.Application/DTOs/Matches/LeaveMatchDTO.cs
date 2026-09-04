namespace FourLines.Application.DTOs.Matches;

public record LeaveMatchDTO
{
    public Guid MatchId { get; init; }
    public Guid UserId { get; init; }
}
