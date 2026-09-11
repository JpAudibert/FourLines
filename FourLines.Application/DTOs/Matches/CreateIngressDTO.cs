using FourLines.Application.DTOs.Matches.Interfaces;

namespace FourLines.Application.DTOs.Matches;

public record CreateIngressDTO : ICreateIngressDTO
{
    public Guid MatchId { get; init; }
    public Guid UserId { get; init; }
    public string Code { get; init; } = default!;
    public bool IngressAsGoalKeeper { get; init; }
}
