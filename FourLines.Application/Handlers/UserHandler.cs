namespace FourLines.Application.Handlers;

public class UserHandler(FourLinesContext context, IPasswordHashProvider passwordHashProvider)
{
    private readonly FourLinesContext _context = context;
    private readonly IPasswordHashProvider _passwordHashProvider = passwordHashProvider;

    public async Task<Result<User>> Create(UserRegisterDTO request)
    {
        User? existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser is not null)
            return Result<User>.Failure(UsersErrorResults.EmailAlreadyExists);

        Role? role = _context.Roles.FirstOrDefault(r => r.Id == request.RoleId);
        if (role is null)
            return Result<User>.Failure(UsersErrorResults.InvalidRole);

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
