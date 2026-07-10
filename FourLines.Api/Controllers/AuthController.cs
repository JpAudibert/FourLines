namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController(AuthenticationHandler authenticationHandler) : ControllerBase
{
    private readonly AuthenticationHandler _authenticationHandler = authenticationHandler;

    [HttpPost]
    public async Task<ActionResult<string>> Authenticate(LoginViewModel request)
    {
        Result<string> result = await _authenticationHandler.Authenticate(new AuthenticationDTO()
        {
            Email = request.Email,
            Password = request.Password,
        });

        if (result.IsFailure)
            Problem(
                title: result.Error.Code,
                detail: result.Error.Description,
                statusCode: StatusCodes.Status401Unauthorized);

        return result.Value;
    }
}
