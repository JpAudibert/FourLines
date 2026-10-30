using FourLines.Api.Models.RequestModels.Matches;
using FourLines.Application.DTOs.Matches;
using FourLines.Application.Interfaces;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class MatchesController(
    ILogger<MatchesController> logger,
    IMatchHandler matchHandler,
    ICurrentUserProvider currentUserProvider
) : ApiControllerBase(logger, currentUserProvider)
{
    [HttpGet("{matchId}")]
    public async Task<ActionResult<Match>> GetMatch(
        [FromRoute] Guid matchId,
        CancellationToken cancellationToken = default
    )
    {
        const string operation = $"{nameof(MatchesController)}.{nameof(GetMatch)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object> { ["operation"] = operation, ["matchId"] = matchId }
        );

        StartStopwatch();

        Result<Match> match = await matchHandler.GetMatch(matchId, cancellationToken);

        return HandleResult(match);
    }

    [HttpPost("{matchId}/ingress")]
    public async Task<ActionResult<MatchesUsers>> Ingress(
        [FromRoute] Guid matchId,
        [FromBody] CreateIngressRequest newIngress,
        CancellationToken cancellationToken = default
    )
    {
        if (!TryGetCurrentUserId(out Guid userId))
            return Unauthorized();

        const string operation = $"{nameof(MatchesController)}.{nameof(Ingress)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["matchId"] = matchId,
                ["userId"] = userId,
            }
        );

        StartStopwatch();

        CreateIngressDTO ingress = new()
        {
            MatchId = matchId,
            UserId = userId,
            Code = newIngress.Code,
        };

        Result<MatchesUsers> matchesUsers = await matchHandler.Ingress(ingress, cancellationToken);

        return HandleResult(matchesUsers);
    }

    [HttpPost("{matchId}/ingress-as-fixed-position")]
    public async Task<ActionResult<MatchesUsers>> IngressAsFixedPosition(
        [FromRoute] Guid matchId,
        [FromBody] CreateIngressRequest newIngress,
        CancellationToken cancellationToken = default
    )
    {
        if (!TryGetCurrentUserId(out Guid userId))
            return Unauthorized();

        const string operation = $"{nameof(MatchesController)}.{nameof(IngressAsFixedPosition)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["matchId"] = matchId,
                ["userId"] = userId,
            }
        );

        StartStopwatch();

        CreateIngressDTO ingress = new()
        {
            MatchId = matchId,
            UserId = userId,
            Code = newIngress.Code,
        };

        Result<MatchesUsers> matchesUsers = await matchHandler.IngressAsFixedPosition(
            ingress,
            cancellationToken
        );

        return HandleResult(matchesUsers);
    }

    [HttpDelete("{matchId}/leave")]
    public async Task<ActionResult<bool>> LeaveMatch(
        [FromRoute] Guid matchId,
        CancellationToken cancellationToken = default
    )
    {
        if (!TryGetCurrentUserId(out Guid userId))
            return Unauthorized();

        const string operation = $"{nameof(MatchesController)}.{nameof(LeaveMatch)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["matchId"] = matchId,
                ["userId"] = userId,
            }
        );

        StartStopwatch();

        LeaveMatchDTO leaveMatch = new() { MatchId = matchId, UserId = userId };

        Result<bool> matchesUsers = await matchHandler.LeaveMatch(leaveMatch, cancellationToken);

        return HandleResult(matchesUsers);
    }

    [HttpPatch("{matchId}/update-match-name")]
    public async Task<ActionResult<Match>> UpdateMatchName(
        [FromRoute] Guid matchId,
        [FromBody] UpdateMatchNameRequest updateMatchName,
        CancellationToken cancellationToken = default
    )
    {
        if (!TryGetCurrentUser(out Guid userId, out bool isAdmin))
            return Unauthorized();

        const string operation = $"{nameof(MatchesController)}.{nameof(UpdateMatchName)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object> { ["operation"] = operation, ["matchId"] = matchId }
        );

        StartStopwatch();

        UpdateMatchNameDTO updateMatchNameDTO = new()
        {
            MatchId = matchId,
            UserId = userId,
            IsAdmin = isAdmin,
            NewName = updateMatchName.Name,
        };

        Result<Match> match = await matchHandler.UpdateMatchName(
            updateMatchNameDTO,
            cancellationToken
        );

        return HandleResult(match);
    }
}
