using FourLines.Application.Authentication.DTOs;
using FourLines.Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController(FourLinesContext fourLinesContext, ITokenProvider tokenProvider) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<string>> Authenticate(LoginRequest request)
    {
        User? user = await fourLinesContext.Users.FirstOrDefaultAsync(user => user.Email == request.Email);

        if (user is null)
            return Unauthorized("Action cannot be proceeded");

        // TODO : Implement password hashing and verification

        return tokenProvider.Create(user);
    }
        
}
