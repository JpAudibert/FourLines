using FourLines.Application.DTOs.Matches.Interfaces;

namespace FourLines.Tests.Matches;

public record TestCreateIngressDTO : ICreateIngressDTO
{
    public Guid MatchId { get; init; }
    public Guid UserId { get; init; }
    public string Code { get; init; } = default!;
    public bool IngressAsGoalKeeper { get; init; }
}