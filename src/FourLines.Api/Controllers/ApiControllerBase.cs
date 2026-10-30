namespace FourLines.Api.Controllers;

public class ApiControllerBase(ILogger logger, ICurrentUserProvider currentUserProvider)
    : ControllerBase
{
    private readonly Stopwatch _sw = new();

    [NonAction]
    public virtual bool TryGetCurrentUser(out Guid userId, out bool isAdmin)
    {
        Guid? currentUserId = currentUserProvider.UserId;
        if (currentUserId is Guid id)
        {
            userId = id;
            isAdmin = currentUserProvider.IsAdmin;
            return true;
        }

        userId = default;
        isAdmin = false;
        return false;
    }

    protected bool TryGetCurrentUserId(out Guid userId) =>
        TryGetCurrentUser(out userId, out _);

    protected void StartStopwatch()
    {
        _sw.Restart();
    }

    protected ActionResult<T> HandleResult<T>(
        Result<T> result,
        int succeedingStatusCode = StatusCodes.Status200OK,
        int failingStatusCode = StatusCodes.Status400BadRequest
    )
    {
        if (result.IsFailure)
        {
            logger.LogWarning(
                "Failed with result: {code} - {description}",
                result.Error.Code,
                result.Error.Description
            );

            return Problem(
                title: result.Error.Code,
                detail: result.Error.Description,
                statusCode: failingStatusCode
            );
        }

        logger.LogDebug("Executed in {ms}", _sw.ElapsedMilliseconds);

        return StatusCode(succeedingStatusCode, result.Value);
    }
}
