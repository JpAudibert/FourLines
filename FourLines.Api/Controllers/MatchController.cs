using FourLines.Api.ViewModels.Matches;
using FourLines.Application.DTOs.Matches;
using FourLines.Application.Interfaces;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class MatchController(ILogger<MatchController> logger, IMatchHandler matchHandler) : ApiControllerBase(logger)
{
    [HttpGet("{matchId}")]
    public async Task<ActionResult<Match>> GetMatch([FromRoute] Guid matchId)
    {
        const string operation = $"{nameof(MatchController)}.{nameof(GetMatch)}";
        using var scope = logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["matchId"] = matchId,
        });

        StartStopwatch();

        Result<Match> match = await matchHandler.GetMatch(matchId);

        return HandleResult(match);
    }

    [HttpPost("{matchId}/ingress/{userId}")]
    public async Task<ActionResult<MatchesUsers>> Ingress(
        [FromRoute] Guid matchId,
        [FromRoute] Guid userId,
        [FromBody] CreateIngressViewModel newIngress)
    {
        const string operation = $"{nameof(MatchController)}.{nameof(GetMatch)}";
        using var scope = logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["matchId"] = matchId,
            ["userId"] = userId,
        });

        StartStopwatch();

        CreateIngressDTO ingress = new()
        {
            MatchId = matchId,
            UserId = userId,
            Code = newIngress.Code,
            IngressAsGoalKeeper = false
        };

        Result<MatchesUsers> matchesUsers = await matchHandler.Ingress(ingress);

        return HandleResult(matchesUsers);
    }

    [HttpPost("{matchId}/ingress/{userId}/ingress-as-goal-keeper")]
    public async Task<ActionResult<MatchesUsers>> IngressAsGoalKeeper(
        [FromRoute] Guid matchId,
        [FromRoute] Guid userId,
        [FromBody] CreateIngressViewModel newIngress)
    {
        const string operation = $"{nameof(MatchController)}.{nameof(GetMatch)}";
        using var scope = logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["matchId"] = matchId,
            ["userId"] = userId,
        });

        StartStopwatch();

        CreateIngressDTO ingress = new()
        {
            MatchId = matchId,
            UserId = userId,
            Code = newIngress.Code,
            IngressAsGoalKeeper = true
        };

        Result<MatchesUsers> matchesUsers = await matchHandler.IngressAsGoalKeeper(ingress);

        return HandleResult(matchesUsers);
    }

    [HttpDelete("{matchId}/leave/{userId}")]
    public async Task<ActionResult<bool>> LeaveMatch(
        [FromRoute] Guid matchId,
        [FromRoute] Guid userId)
    {
        const string operation = $"{nameof(MatchController)}.{nameof(LeaveMatch)}";
        using var scope = logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["matchId"] = matchId,
            ["userId"] = userId,
        });

        StartStopwatch();

        LeaveMatchDTO leaveMatch = new()
        {
            MatchId = matchId,
            UserId = userId
        };

        Result<bool> matchesUsers = await matchHandler.LeaveMatch(leaveMatch);

        return HandleResult(matchesUsers);
    }

    [HttpPatch("{matchId}/update-match-name")]
    public async Task<ActionResult<Match>> UpdateMatchName(
        [FromRoute] Guid matchId,
        [FromBody] UpdateMatchNameViewModel updateMatchName)
    {
        const string operation = $"{nameof(MatchController)}.{nameof(UpdateMatchName)}";
        using var scope = logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["matchId"] = matchId,
        });

        StartStopwatch();

        UpdateMatchNameDTO updateMatchNameDTO = new()
        {
            MatchId = matchId,
            NewName = updateMatchName.Name
        };

        Result<Match> match = await matchHandler.UpdateMatchName(updateMatchNameDTO);

        return HandleResult(match);
    }
}
