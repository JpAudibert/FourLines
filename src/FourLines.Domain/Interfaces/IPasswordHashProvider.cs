namespace FourLines.Domain.Interfaces;

public interface IPasswordHashProvider
{
    string Hash(User user, string password);
    bool VerifyHashedPassword(User user, string hashedPassword, string password);
}
