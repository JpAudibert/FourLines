using FourLines.Api.ViewModels;
using FourLines.Application.DTOs;
using FourLines.Application.Handlers;
using FourLines.Domain.Results;
using Microsoft.AspNetCore.Mvc;

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

        if(result.IsFailure)
            return Unauthorized(result.Error);

        return result.Value;
    }
}
