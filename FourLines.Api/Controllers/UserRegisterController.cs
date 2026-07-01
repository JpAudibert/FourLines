using FourLines.Api.ViewModels;
using FourLines.Application.DTOs;
using FourLines.Application.Handlers;
using FourLines.Domain.Results;
using Microsoft.AspNetCore.Mvc;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserRegisterController(UserHandler userHandler) : ControllerBase
{
    private readonly UserHandler _userHandler = userHandler;

    [HttpPost]
    public async Task<ActionResult<User>> Register([FromBody] UserRegisterViewModel request)
    {
        Result<User> result = await _userHandler.Create(new UserRegisterDTO
        {
            Name = request.Name,
            Email = request.Email,
            Birthday = request.Birthday,
            Phone = request.Phone,
            RegistrationNumber = request.RegistrationNumber,
            RoleName = request.RoleName,
            Password = request.Password,
            isActive = request.isActive
        });

        if(result.IsFailure)
            return BadRequest(result.Error);

        return result.Value;
    }
}
