namespace FourLines.Domain.Interfaces;

public interface ITokenProvider
{
    string Create(User user);
}
