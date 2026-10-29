using FourLines.Application.DTOs.Users;

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

        Role role = await GetRole(request.IsFacilityOwner, cancellationToken);
        User user = new()
        {
            Name = request.Name,
            Email = request.Email,
            Birthday = request.Birthday,
            Phone = request.Phone,
            RegistrationNumber = request.RegistrationNumber,
            // remove after email confirmation is implemented
            IsActive = request.IsActive,
            RoleId = role.Id,
            Role = role,
        };
        user.PasswordHash = passwordHashProvider.Hash(user, request.Password);

        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result<User>.Success(user);
    }

    public async Task<Result<bool>> Delete(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        int affectedRows = await context
            .Users.Where(u => u.Id == userId)
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);

        if (affectedRows <= 0)
            return Result<bool>.Failure(UsersErrorResults.UserNotFound);

        await context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }

    public async Task<Result<User>> GetById(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        User? user = await context.Users.FirstOrDefaultAsync(
            u => u.Id == userId,
            cancellationToken: cancellationToken
        );
        if (user is null)
            return Result<User>.Failure(UsersErrorResults.UserNotFound);

        return Result<User>.Success(user);
    }

    public async Task<Result<User>> Update(
        Guid userId,
        UserUpdateDTO request,
        CancellationToken cancellationToken = default
    )
    {
        User? user = await context.Users.FirstOrDefaultAsync(
            u => u.Id == userId,
            cancellationToken: cancellationToken
        );
        if (user is null)
            return Result<User>.Failure(UsersErrorResults.UserNotFound);

        int affectedRows = await context
            .Users.Where(u => u.Id == userId)
            .ExecuteUpdateAsync(
                u =>
                    u.SetProperty(u => u.Name, request.Name)
                        .SetProperty(u => u.Birthday, request.Birthday)
                        .SetProperty(u => u.Phone, request.Phone)
                        .SetProperty(u => u.RegistrationNumber, request.RegistrationNumber)
                        .SetProperty(u => u.IsActive, request.IsActive),
                cancellationToken: cancellationToken
            );

        if (affectedRows <= 0)
            return Result<User>.Failure(UsersErrorResults.UserNotFound);

        await context.SaveChangesAsync(cancellationToken);

        User updatedUser = await context.Users.FirstAsync(
            u => u.Id == userId,
            cancellationToken: cancellationToken
        );

        updatedUser.Name = request.Name;
        updatedUser.Birthday = request.Birthday;
        updatedUser.Phone = request.Phone;
        updatedUser.RegistrationNumber = request.RegistrationNumber;
        updatedUser.IsActive = request.IsActive;

        return Result<User>.Success(updatedUser);
    }

    protected virtual async Task<Role> GetRole(
        bool isFacilityOwner,
        CancellationToken cancellationToken
    )
    {
        string roleName = isFacilityOwner ? RoleConstants.FacilityOwner : RoleConstants.Player;

        return await context.Roles.FirstAsync(
            r => r.Name == roleName,
            cancellationToken: cancellationToken
        );
    }
}
