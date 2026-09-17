namespace FourLines.Domain.Interfaces;

public interface IShuffleStrategy
{
    IEnumerable<MatchesUsers> Shuffle(IEnumerable<MatchesUsers> players);
}