using FourLines.Application.DTOs.Matches;

namespace FourLines.Application.Interfaces;

public interface IMatchHandler
{
    Task<Result<Match>> GetMatch(
        Guid matchId,
        Guid userId,
        bool isAdmin,
        CancellationToken cancellationToken = default
    );
    Task<Result<MatchesUsers>> Ingress(
        CreateIngressDTO ingress,
        CancellationToken cancellationToken = default
    );
    Task<Result<MatchesUsers>> IngressAsFixedPosition(
        CreateIngressDTO ingress,
        CancellationToken cancellationToken = default
    );
    Task<Result<bool>> LeaveMatch(
        LeaveMatchDTO leaveMatch,
        CancellationToken cancellationToken = default
    );
    Task<Result<Match>> UpdateMatchName(
        UpdateMatchNameDTO updateMatchName,
        CancellationToken cancellationToken = default
    );
}
