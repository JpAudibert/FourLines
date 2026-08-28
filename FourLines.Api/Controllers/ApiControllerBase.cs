namespace FourLines.Api.Controllers
{
    public class ApiControllerBase(ILogger logger) : ControllerBase
    {
        private readonly ILogger _logger = logger;
        private readonly Stopwatch _sw = new();

        protected void StartStopwatch()
        {
            _sw.Restart();
        }

        protected ActionResult<T> HandleResult<T>(
            Result<T> result,
            int failingStatusCodes = StatusCodes.Status400BadRequest)
        {
            if (result.IsFailure)
            {
                _logger.LogWarning("Failed with result: {code} - {description}",
                    result.Error.Code,
                    result.Error.Description);

                return Problem(
                    title: result.Error.Code,
                    detail: result.Error.Description,
                    statusCode: failingStatusCodes);
            }

            _logger.LogDebug("Executed in {ms}", _sw.ElapsedMilliseconds);

            return result.Value;
        }
    }
}
