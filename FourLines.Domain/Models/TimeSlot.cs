namespace FourLines.Domain.Models;

public record TimeSlot
{
    public TimeOnly Start { get; init; }
    public TimeOnly End { get; init; }
    public Money Price { get; init; } = default!;
    public bool Available { get; set; }
}
