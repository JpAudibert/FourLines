namespace FourLines.Api.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected ActionResult<T> HandleResult<T>(
            Result<T> result,
            ILogger logger,
            string operation,
            Stopwatch sw,
            int failingStatusCodes = StatusCodes.Status400BadRequest)
        {
            if (result.IsFailure)
            {
                logger.LogWarning("{op} - Failed with result: {code} - {description}",
                    operation,
                    result.Error.Code,
                    result.Error.Description);

                return Problem(
                    title: result.Error.Code,
                    detail: result.Error.Description,
                    statusCode: failingStatusCodes);
            }

            logger.LogInformation("{op} - executed in {ms}", operation, sw.ElapsedMilliseconds);

            return result.Value;
        }
    }
}
