using FourLines.Application.Interfaces;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/matches/{matchId}/[controller]")]
public class ShuffleController(ILogger<ShuffleController> logger, IShuffleHandler shuffleHandler)
    : ApiControllerBase(logger)
{
    [HttpPost]
    public async Task<ActionResult<IEnumerable<MatchesUsers>>> ShuffleTeams(
        [FromRoute] Guid matchId
    )
    {
        const string operation = $"{nameof(ReservationsController)}.{nameof(ShuffleTeams)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object> { ["operation"] = operation, ["matchId"] = matchId }
        );

        StartStopwatch();

        Result<IEnumerable<MatchesUsers>> result = await shuffleHandler.ShufflePlayers(matchId);

        return HandleResult(result);
    }
}
