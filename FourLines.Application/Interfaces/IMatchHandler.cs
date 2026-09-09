using FourLines.Application.DTOs.Matches;
using FourLines.Application.DTOs.Matches.Interfaces;

namespace FourLines.Application.Interfaces
{
    public interface IMatchHandler
    {
        Task<Result<Match>> GetMatch(Guid matchId);
        Task<Result<MatchesUsers>> Ingress(ICreateIngressDTO ingress);
        Task<Result<MatchesUsers>> IngressAsGoalKeeper(ICreateIngressDTO ingress);
        Task<Result<bool>> LeaveMatch(LeaveMatchDTO leaveMatch);
        Task<Result<Match>> UpdateMatchName(UpdateMatchNameDTO updateMatchName);
    }
}