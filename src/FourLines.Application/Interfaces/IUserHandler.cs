using FourLines.Application.DTOs.Users;

namespace FourLines.Application.Interfaces;

public interface IUserHandler
{
    Task<Result<User>> Create(
        UserRegisterDTO request,
        CancellationToken cancellationToken = default
    );

    Task<Result<User>> GetById(Guid userId, CancellationToken cancellationToken = default);
    Task<Result<User>> Update(
        Guid userId,
        UserUpdateDTO request,
        CancellationToken cancellationToken = default
    );

    Task<Result<bool>> Delete(Guid userId, CancellationToken cancellationToken = default);
}
