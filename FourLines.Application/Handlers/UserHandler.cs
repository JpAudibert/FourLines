using FourLines.Application.DTOs;
using FourLines.Infrastructure.Contexts;

namespace FourLines.Application.Handlers;

public class UserHandler(FourLinesContext context, IPasswordHashProvider passwordHashProvider)
{
    private readonly FourLinesContext _context = context;
    private readonly IPasswordHashProvider _passwordHashProvider = passwordHashProvider;

    public async Task<User> Create(UserRegisterDTO request)
    {
        using (_context)
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            var role = _context.Roles.FirstOrDefault(r => r.Name == request.RoleName) ?? throw new Exception("Role not found");

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

            await transaction.CommitAsync();

            return user;
        }

    }
}
