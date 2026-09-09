namespace FourLines.Application.DTOs.Matches.Interfaces
{
    public interface ICreateIngressDTO
    {
        string Code { get; init; }
        bool IngressAsGoalKeeper { get; init; }
        Guid MatchId { get; init; }
        Guid UserId { get; init; }
    }
}