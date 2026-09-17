namespace FourLines.Application.Interfaces;

public interface IShuffleHandler
{
    Task<Result<IEnumerable<MatchesUsers>>> ShufflePlayers(Guid matchId);
}
