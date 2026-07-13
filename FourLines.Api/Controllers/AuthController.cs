namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController(ILogger<AuthController> logger, AuthenticationHandler authenticationHandler) : ApiControllerBase
{
    private readonly ILogger<AuthController> _logger = logger;
    private readonly AuthenticationHandler _authenticationHandler = authenticationHandler;

    [HttpPost]
    public async Task<ActionResult<string>> Authenticate(LoginViewModel request)
    {
        const string operation = nameof(Authenticate);
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
        });

        Stopwatch sw = Stopwatch.StartNew();

        _logger.LogInformation("{op} - Authentication for {identification}",
            operation,
            request.Email);

        Result<string> result = await _authenticationHandler.Authenticate(new AuthenticationDTO()
        {
            Email = request.Email,
            Password = request.Password,
        });

        return HandleResult(result, _logger, operation, sw);
    }
}
