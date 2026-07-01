using FourLines.Application.DTOs;
using FourLines.Domain.Results;
using FourLines.Domain.Results.User;
using FourLines.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace FourLines.Application.Handlers;

public class UserHandler(FourLinesContext context, IPasswordHashProvider passwordHashProvider)
{
    private readonly FourLinesContext _context = context;
    private readonly IPasswordHashProvider _passwordHashProvider = passwordHashProvider;

    public async Task<Result<User>> Create(UserRegisterDTO request)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser is not null)
            return Result<User>.Failure(UserCreationErrorResults.EmailAlreadyExists);

        var role = _context.Roles.FirstOrDefault(r => r.Name == request.RoleName);
        if (role is null)
            return Result<User>.Failure(UserCreationErrorResults.InvalidRole);

        User user = new()
        {
            Name = request.Name,
            Email = request.Email,
            Birthday = request.Birthday,
            Phone = request.Phone,
            RegistrationNumber = request.RegistrationNumber,
            RoleId = role.Id,
            Role = role,
        };
        user.PasswordHash = _passwordHashProvider.Hash(user, request.Password);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return Result<User>.Success(user);
    }
}
