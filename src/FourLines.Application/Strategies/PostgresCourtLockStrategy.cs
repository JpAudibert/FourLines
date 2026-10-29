namespace FourLines.Application.Strategies;

public class PostgresCourtLockStrategy(FourLinesContext context) : ICourtLockStrategies
{
    public async Task<Court?> GetForUpdateAsync(Guid courtId, CancellationToken cancellationToken = default)
    {
        Court? court = await context.Courts
            .FromSqlInterpolated($@"
                SELECT 
                    id,
                    facility_id,
                    sport_id,
                    name,
                    is_active,
                    amount AS ""DefaultPrice_Amount"",
                    currency AS ""DefaultPrice_Currency"",
                    maintenance_period_in_minutes,
                    renting_period_in_minutes,
                    created_at,
                    updated_at
                  FROM courts 
                 WHERE Id = {courtId} 
                   FOR UPDATE"
        ).SingleOrDefaultAsync(cancellationToken);

        return court;
    }
}
