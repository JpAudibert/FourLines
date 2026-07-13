namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController(ILogger<AuthController> logger, AuthenticationHandler authenticationHandler) : ControllerBase
{
    private readonly ILogger<AuthController> _logger = logger;
    private readonly AuthenticationHandler _authenticationHandler = authenticationHandler;

    [HttpPost]
    public async Task<ActionResult<string>> Authenticate(LoginViewModel request)
    {
        _logger.LogInformation("Authentication for {identification}", request.Email);
        Result<string> result = await _authenticationHandler.Authenticate(new AuthenticationDTO()
        {
            Email = request.Email,
            Password = request.Password,
        });

        if (result.IsFailure)
        {
            _logger.LogInformation("{process} failed with this result: {code} - {description}",
                nameof(AuthController),
                result.Error.Code,
                result.Error.Description);
            Problem(
                title: result.Error.Code,
                detail: result.Error.Description,
                statusCode: StatusCodes.Status401Unauthorized);
        }

        _logger.LogInformation("{process} succeeded", nameof(AuthController));

        return result.Value;
    }
}
