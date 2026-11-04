namespace FourLines.Application.Providers;

public class PasswordHashProvider(PasswordHasher<User> passwordHasher) : IPasswordHashProvider
{
    public string Hash(User user, string password)
    {
        return passwordHasher.HashPassword(user, password);
    }

    public bool VerifyHashedPassword(User user, string hashedPassword, string password)
    {
        var result = passwordHasher.VerifyHashedPassword(user, hashedPassword, password);
        return result == PasswordVerificationResult.Success;
    }
}
