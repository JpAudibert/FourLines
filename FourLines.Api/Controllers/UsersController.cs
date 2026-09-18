using FourLines.Application.Interfaces;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController(ILogger<UsersController> logger, IUserHandler userHandler)
    : ApiControllerBase(logger)
{
    [HttpPost("{roleId}")]
    public async Task<ActionResult<User>> Register(
        [FromRoute] Guid roleId,
        [FromBody] UserRegisterViewModel request,
        CancellationToken cancellationToken
    )
    {
        const string operation = $"{nameof(UsersController)}.{nameof(Register)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["roleId"] = roleId,
                ["email"] = request.Email,
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
                RoleId = roleId,
                Password = request.Password,
                IsActive = request.IsActive,
            },
            cancellationToken
        );

        return HandleResult(result);
    }
}
