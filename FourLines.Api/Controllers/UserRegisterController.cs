using FourLines.Api.ViewModels;
using FourLines.Application.DTOs;
using FourLines.Application.Handlers;
using FourLines.Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserRegisterController(FourLinesContext context, UserHandler userHandler) : ControllerBase
{
    private readonly FourLinesContext _context = context;
    private readonly UserHandler _userHandler = userHandler;

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] UserRegisterViewModel request)
    {
        using (_context)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser is not null)
            {
                return Conflict("User with this email already exists.");
            }

            User user = await _userHandler.Create(new UserRegisterDTO
            {
                Name = request.Name,
                Email = request.Email,
                Birthday = request.Birthday,
                Phone = request.Phone,
                RegistrationNumber = request.RegistrationNumber,
                RoleName = request.RoleName,
                Password = request.Password
            });

            return Ok(user);
        }
    }
}
