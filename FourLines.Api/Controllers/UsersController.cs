using FourLines.Application.Interfaces;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController(ILogger<UsersController> logger, IUserHandler userHandler)
    : ApiControllerBase(logger)
{
    [HttpPost("players")]
    public async Task<ActionResult<User>> RegisterPlayer(
        [FromBody] UserRegisterViewModel request,
        CancellationToken cancellationToken
    )
    {
        return await RegisterUser(request, isFacilityOwner: false, cancellationToken);
    }

    [HttpPost("facility-owners")]
    public async Task<ActionResult<User>> RegisterFacilityOwner(
        [FromBody] UserRegisterViewModel request,
        CancellationToken cancellationToken
    )
    {
        return await RegisterUser(request, isFacilityOwner: true, cancellationToken);
    }

    protected virtual async Task<ActionResult<User>> RegisterUser(
        UserRegisterViewModel request,
        bool isFacilityOwner,
        CancellationToken cancellationToken
    )
    {
        const string operation = $"{nameof(UsersController)}.{nameof(RegisterFacilityOwner)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["email"] = request.Email,
                ["isFacilityOwner"] = isFacilityOwner,
            }
        );

        StartStopwatch();

        Result<User> result = await userHandler.Create(
            new UserRegisterDTO
            {
                Name = request.Name,
                Email = request.Email,
                Birthday = request.Birthday,
                Phone = request.Phone,
                RegistrationNumber = request.RegistrationNumber,
                IsFacilityOwner = true,
                Password = request.Password,
                IsActive = request.IsActive,
            },
            cancellationToken
        );

        return HandleResult(result);
    }
}
