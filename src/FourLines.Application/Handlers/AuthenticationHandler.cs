namespace FourLines.Application.Handlers;

public class AuthenticationHandler(
    FourLinesContext context,
    IPasswordHashProvider passwordHashProvider,
    ITokenProvider tokenProvider
) : IAuthenticationHandler
{
    public async Task<Result<string>> Authenticate(
        AuthenticationDTO request,
        CancellationToken cancellationToken = default
    )
    {
        User? user = await context
            .Users.Include(user => user.Role)
            .FirstOrDefaultAsync(
                user => user.Email == request.Email,
                cancellationToken: cancellationToken
            );

        if (user is null)
            return Result<string>.Failure(AuthenticationErrorResults.UnknownUser);

        bool arePasswordsEqual = passwordHashProvider.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );

        if (!arePasswordsEqual)
            return Result<string>.Failure(AuthenticationErrorResults.InvalidPassword);

        string token = tokenProvider.Create(user);

        return Result<string>.Success(token);
    }
}
