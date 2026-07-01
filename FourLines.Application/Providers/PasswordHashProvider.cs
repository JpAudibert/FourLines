namespace FourLines.Application.Providers;

public class PasswordHashProvider(PasswordHasher<User> passwordHasher) : IPasswordHashProvider
{
    private readonly PasswordHasher<User> _passwordHasher = passwordHasher;

    public string Hash(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyHashedPassword(User user, string hashedPassword, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, password);
        return result == PasswordVerificationResult.Success;
    }
}
