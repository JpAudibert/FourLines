namespace FourLines.Application.Interfaces;

public interface ICourtLockStrategies
{
    Task<Court?> GetForUpdateAsync(Guid courtId, CancellationToken cancellationToken = default);
}
