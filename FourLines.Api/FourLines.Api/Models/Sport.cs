namespace FourLines.Api.Models;

public class Sport : BaseEntity
{
    public string Name { get; init; } = default!;
    public bool Indoor { get; init; }
    public int StartingPlayersCount { get; init; }
    public int MaxPlayersCount { get; init; }

    public IEnumerable<Court> Courts { get; init; } = [];
}
