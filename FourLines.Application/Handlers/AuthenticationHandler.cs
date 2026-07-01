namespace FourLines.Application.Handlers;

public class AuthenticationHandler(FourLinesContext context, IPasswordHashProvider passwordHashProvider, ITokenProvider tokenProvider)
{
    private readonly FourLinesContext _context = context;
    private readonly IPasswordHashProvider _passwordHashProvider = passwordHashProvider;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public async Task<Result<string>> Authenticate(AuthenticationDTO request)
    {
        User? user = await _context.Users.FirstOrDefaultAsync(user => user.Email == request.Email);

        if (user is null)
            return Result<string>.Failure(AuthenticationErrorResults.UnknownUser);

        bool arePasswordsEqual = _passwordHashProvider.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (!arePasswordsEqual)
            return Result<string>.Failure(AuthenticationErrorResults.InvalidPassword);

        string token = _tokenProvider.Create(user);

        return Result<string>.Success(token);
    }
}
