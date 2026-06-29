using FourLines.Api.ViewModels;
using FourLines.Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController(FourLinesContext fourLinesContext, ITokenProvider tokenProvider, IPasswordHashProvider passwordHashProvider) : ControllerBase
{
    private readonly FourLinesContext _fourLinesContext = fourLinesContext;
    private readonly ITokenProvider _tokenProvider = tokenProvider;
    private readonly IPasswordHashProvider _passwordHashProvider = passwordHashProvider;

    [HttpPost]
    public async Task<ActionResult<string>> Authenticate(LoginViewModel request)
    {
        using (_fourLinesContext)
        {
            User? user = await _fourLinesContext.Users.FirstOrDefaultAsync(user => user.Email == request.Email);

            if (user is null)
                return Unauthorized("Action cannot be proceeded");

            bool arePasswordsEqual = _passwordHashProvider.VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (!arePasswordsEqual)
                return Unauthorized("Action cannot be proceeded");

            return _tokenProvider.Create(user);
        }
    }

}
