namespace FourLines.Application.Interfaces;

public interface IUserHandler
{
    Task<Result<User>> Create(
        UserRegisterDTO request,
        CancellationToken cancellationToken = default
    );
}
