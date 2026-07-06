namespace FourLines.Domain.Models;

public class Reservation : BaseEntity
{
    public Guid CourtId { get; init; }
    public Guid UserId { get; init; }
    public TimeRange Period { get; init; } = default!;
    public ReservationStatus Status { get; init; } = default!;
}

public enum ReservationStatus
{
    Pending,
    Confirmed,
    Cancelled
}

public record TimeRange(DateTime Start, DateTime End)
{
    public TimeSpan Duration => End - Start;

    public bool AreValidDates() => End > Start;

    public bool Overlaps(TimeRange other) => Start < other.End && End > other.Start;

    public bool Contains(DateTime instant) => instant >= Start && instant < End;
    public bool Contains(TimeRange other) => Start <= other.Start && End >= other.End;

    public bool Touches(TimeRange other) => End == other.Start || Start == other.End;

}
