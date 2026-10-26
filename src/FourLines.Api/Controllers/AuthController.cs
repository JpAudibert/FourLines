using FourLines.Application.Interfaces;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController(
    ILogger<AuthController> logger,
    IAuthenticationHandler authenticationHandler
) : ApiControllerBase(logger)
{
    [HttpPost]
    public async Task<ActionResult<string>> Authenticate(
        LoginViewModel request,
        CancellationToken cancellationToken
    )
    {
        const string operation = nameof(Authenticate);
        using var scope = logger.BeginScope(
            new Dictionary<string, object> { ["operation"] = operation }
        );

        StartStopwatch();

        logger.LogInformation("Authentication for {identification}", request.Email);

        Result<string> result = await authenticationHandler.Authenticate(
            new AuthenticationDTO() { Email = request.Email, Password = request.Password },
            cancellationToken
        );

        return HandleResult(result, StatusCodes.Status401Unauthorized);
    }
}
