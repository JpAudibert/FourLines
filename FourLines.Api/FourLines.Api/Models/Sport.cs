namespace FourLines.Api.Models;

public class Sport : BaseEntity
{
    public string Name { get; set; } = default!;
    public bool Indoor { get; set; }
    public int StartingPlayersCount { get; set; }
    public int MaxPlayersCount { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public IEnumerable<Court> Courts { get; set; } = [];
}
