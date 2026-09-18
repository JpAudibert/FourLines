using FourLines.Application.DTOs.Matches;
using FourLines.Application.DTOs.Matches.Interfaces;

namespace FourLines.Application.Interfaces;

public interface IMatchHandler
{
    Task<Result<Match?>> GetMatch(Guid matchId, CancellationToken cancellationToken = default);
    Task<Result<MatchesUsers>> Ingress(
        ICreateIngressDTO ingress,
        CancellationToken cancellationToken = default
    );
    Task<Result<MatchesUsers>> IngressAsFixedPosition(
        ICreateIngressDTO ingress,
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
