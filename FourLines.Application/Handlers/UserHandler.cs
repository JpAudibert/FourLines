namespace FourLines.Application.Handlers;

public class UserHandler(FourLinesContext context, IPasswordHashProvider passwordHashProvider)
    : IUserHandler
{
    public async Task<Result<User>> Create(
        UserRegisterDTO request,
        CancellationToken cancellationToken = default
    )
    {
        User? existingUser = await context.Users.FirstOrDefaultAsync(
            u => u.Email == request.Email,
            cancellationToken: cancellationToken
        );
        if (existingUser is not null)
            return Result<User>.Failure(UsersErrorResults.EmailAlreadyExists);

        Role? role = context.Roles.FirstOrDefault(r => r.Id == request.RoleId);
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
        user.PasswordHash = passwordHashProvider.Hash(user, request.Password);

        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result<User>.Success(user);
    }
}
