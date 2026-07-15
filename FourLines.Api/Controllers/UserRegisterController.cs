namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserRegisterController(ILogger logger, UserHandler userHandler)
    : ApiControllerBase
{
    private readonly ILogger _logger = logger;
    private readonly UserHandler _userHandler = userHandler;

    [HttpPost("{roleId}")]
    public async Task<ActionResult<User>> Register([FromRoute] Guid roleId, [FromBody] UserRegisterViewModel request)
    {
        const string operation = $"{nameof(UserRegisterController)}.{nameof(Register)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["roleId"] = roleId,
            ["email"] = request.Email,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<User> result = await _userHandler.Create(new UserRegisterDTO
        {
            Name = request.Name,
            Email = request.Email,
            Birthday = request.Birthday,
            Phone = request.Phone,
            RegistrationNumber = request.RegistrationNumber,
            RoleId = roleId,
            Password = request.Password,
            IsActive = request.IsActive
        });

        return HandleResult(result, _logger, operation, sw);
    }
}
