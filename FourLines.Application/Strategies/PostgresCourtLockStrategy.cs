namespace FourLines.Application.Strategies;

public class PostgresCourtLockStrategy(FourLinesContext context) : ICourtLockStrategies
{
    public async Task<Court?> GetForUpdateAsync(Guid courtId, CancellationToken cancellationToken = default)
    {
        Court? court = await context.Courts
            .FromSqlInterpolated($@"
                SELECT * 
                  FROM courts 
                 WHERE Id = {courtId} 
                   FOR UPDATE"
        ).SingleOrDefaultAsync(cancellationToken);

        return court;
    }
}
