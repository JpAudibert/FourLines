using FourLines.Domain.Interfaces;

namespace FourLines.Domain.Strategies;

public class DotnetShuffleStrategy : IShuffleStrategy
{
    public IEnumerable<MatchesUsers> Shuffle(IEnumerable<MatchesUsers> players)
    {
        return players.Shuffle();
    }
}
