namespace FourLines.Application.Strategies;

public class SqliteCourtLockStrategy(FourLinesContext context) : ICourtLockStrategies
{
    public async Task<Court?> GetForUpdateAsync(Guid courtId, CancellationToken cancellationToken = default)
    {
        return await context.Courts.SingleOrDefaultAsync(c => c.Id == courtId, cancellationToken);
    }
}
