namespace FourLines.Application.Handlers;

public class ShuffleHandler(FourLinesContext context, IShuffleStrategy shuffleStrategy)
    : IShuffleHandler
{
    public async Task<Result<IEnumerable<MatchesUsers>>> ShufflePlayers(
        Guid matchId,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<MatchesUsers> players = context.MatchesUsers.Where(mu => mu.MatchId == matchId);
        if (!players.Any())
            return Result<IEnumerable<MatchesUsers>>.Failure(
                ShufflingErrorResults.ShufflingUnknownMatch
            );

        IQueryable<MatchesUsers> fixedPositions = players.Where(p => p.IsFixedPosition);
        IQueryable<MatchesUsers> notFixedPositions = players.Where(p => !p.IsFixedPosition);

        IEnumerable<MatchesUsers> shuffledFixedPositions = shuffleStrategy.Shuffle(fixedPositions);
        IEnumerable<MatchesUsers> shuffledNotFixedPositions = shuffleStrategy.Shuffle(
            notFixedPositions
        );

        List<MatchesUsers> shuffledPlayers =
        [
            .. shuffledFixedPositions,
            .. shuffledNotFixedPositions,
        ];

        for (int i = 0; i < shuffledPlayers.Count; i++)
        {
            shuffledPlayers[i].TeamNumber = i % 2 == 0 ? 1 : 2;
        }

        using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        await context
            .MatchesUsers.Where(mu => mu.MatchId == matchId)
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);

        await context.AddRangeAsync(shuffledPlayers, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return Result<IEnumerable<MatchesUsers>>.Success(shuffledPlayers);
    }
}
