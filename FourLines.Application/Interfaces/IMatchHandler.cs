using FourLines.Application.DTOs.Matches;

namespace FourLines.Application.Interfaces
{
    public interface IMatchHandler
    {
        Task<Result<Match>> GetMatch(Guid matchId);
        Task<Result<MatchesUsers>> Ingress(CreateIngressDTO ingress);
        Task<Result<MatchesUsers>> IngressAsGoalKeeper(CreateIngressDTO ingress);
        Task<Result<bool>> LeaveMatch(LeaveMatchDTO leaveMatch);
        Task<Result<Match>> UpdateMatchName(UpdateMatchNameDTO updateMatchName);
    }
}