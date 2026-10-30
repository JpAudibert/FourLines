namespace FourLines.Application.Interfaces;

public interface IShuffleHandler
{
    Task<Result<IEnumerable<MatchesUsers>>> ShufflePlayers(
        Guid matchId,
        Guid userId,
        bool isAdmin,
        CancellationToken cancellationToken = default
    );
}
