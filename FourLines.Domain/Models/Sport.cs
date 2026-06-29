namespace FourLines.Domain.Models;

public class Sport : BaseEntity
{
    public string Name { get; init; } = default!;
    public bool Indoor { get; init; }
    public int StartingPlayersCount { get; init; }
    public int MaxPlayersCount { get; init; }

    public ICollection<Court> Courts { get; set; } = new List<Court>();
}
