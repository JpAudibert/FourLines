namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserRegisterController(UserHandler userHandler) : ControllerBase
{
    private readonly UserHandler _userHandler = userHandler;

    [HttpPost("{roleId}")]
    public async Task<ActionResult<User>> Register([FromRoute] Guid roleId, [FromBody] UserRegisterViewModel request)
    {
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

        if (result.IsFailure)
            return Problem(
                title: result.Error.Code,
                detail: result.Error.Description, 
                statusCode: StatusCodes.Status400BadRequest);

        return result.Value;
    }
}
