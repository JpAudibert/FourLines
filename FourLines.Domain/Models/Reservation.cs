namespace FourLines.Domain.Models;

public class Reservation : BaseEntity
{
    public Guid CourtId { get; init; }
    public Guid UserId { get; init; }
    public TimeRange Period { get; init; } = default!;
    public ReservationStatus Status { get; init; } = default!;

    public Court Court { get; init; } = default!;
    public User User { get; init; } = default!;
}

public enum ReservationStatus
{
    Pending,
    Confirmed,
    Cancelled
}
