namespace FourLines.Domain.Models;

public record CourtAvailability
{
    public Guid CourtId { get; init; } = default!;
    public DateOnly PlayingDate { get; init; }
    public IEnumerable<TimeSlot> TimeSlots { get; init; } = default!;
}
