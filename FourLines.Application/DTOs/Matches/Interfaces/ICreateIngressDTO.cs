namespace FourLines.Application.DTOs.Matches.Interfaces;

public interface ICreateIngressDTO
{
    string Code { get; init; }
    Guid MatchId { get; init; }
    Guid UserId { get; init; }
}