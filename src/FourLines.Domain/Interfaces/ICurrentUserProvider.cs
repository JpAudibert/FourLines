namespace FourLines.Domain.Interfaces;

public interface ICurrentUserProvider
{
    Guid? UserId { get; }
    bool IsAdmin { get; }
}
