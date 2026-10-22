namespace FourLines.Application.Handlers;

public class UserHandler(FourLinesContext context, IPasswordHashProvider passwordHashProvider)
    : IUserHandler
{
    public async Task<Result<User>> Create(
        UserRegisterDTO request,
        CancellationToken cancellationToken = default
    )
    {
        if (
            await context.Users.AnyAsync(
                u => u.Email == request.Email,
                cancellationToken: cancellationToken
            )
        )
            return Result<User>.Failure(UsersErrorResults.EmailAlreadyExists);

        Role? role = await GetRole(request.IsFacilityOwner, cancellationToken);
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

    protected virtual async Task<Role?> GetRole(
        bool isFacilityOwner,
        CancellationToken cancellationToken
    )
    {
        string roleName = isFacilityOwner ? RoleConstants.FacilityOwner : RoleConstants.Player;

        return await context.Roles.FirstOrDefaultAsync(
            r => r.Name == roleName,
            cancellationToken: cancellationToken
        );
    }
}
