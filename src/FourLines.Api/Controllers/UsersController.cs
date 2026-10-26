using FourLines.Application.DTOs.Users;
using FourLines.Application.Interfaces;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UsersController(ILogger<UsersController> logger, IUserHandler userHandler)
    : ApiControllerBase(logger)
{
    [HttpGet("{id}")]
    [Authorize(Roles = "Player, FacilityOwner, Admin")]
    public async Task<ActionResult<User>> GetUser(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        const string operation = $"{nameof(UsersController)}.{nameof(GetUser)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object> { ["operation"] = operation, ["userId"] = id }
        );

        StartStopwatch();

        Result<User> result = await userHandler.GetById(id, cancellationToken);

        return HandleResult(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Player, FacilityOwner, Admin")]
    public async Task<ActionResult<User>> UpdateUser(
        [FromRoute] Guid id,
        [FromBody] UserUpdateViewModel request,
        CancellationToken cancellationToken
    )
    {
        const string operation = $"{nameof(UsersController)}.{nameof(UpdateUser)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object> { ["operation"] = operation, ["userId"] = id }
        );

        StartStopwatch();

        Result<User> result = await userHandler.Update(
            id,
            new UserUpdateDTO
            {
                Name = request.Name,
                Birthday = request.Birthday,
                Phone = request.Phone,
                RegistrationNumber = request.RegistrationNumber,
                IsActive = request.IsActive,
            },
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Player, FacilityOwner, Admin")]
    public async Task<ActionResult<bool>> DeleteUser(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        const string operation = $"{nameof(UsersController)}.{nameof(DeleteUser)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object> { ["operation"] = operation, ["userId"] = id }
        );

        StartStopwatch();

        Result<bool> result = await userHandler.Delete(id, cancellationToken);

        return HandleResult(result);
    }

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
        const string operation = $"{nameof(UsersController)}.{nameof(RegisterUser)}";
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
                IsFacilityOwner = isFacilityOwner,
                Password = request.Password,
                IsActive = request.IsActive,
            },
            cancellationToken
        );

        return HandleResult(result);
    }
}
