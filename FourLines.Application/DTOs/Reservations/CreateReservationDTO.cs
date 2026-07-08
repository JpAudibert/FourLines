namespace FourLines.Application.DTOs.Reservations;

public class CreateReservationDTO
{
    public Guid CourtId { get; init; }
    public Guid UserId { get; init; }
    public TimeRange Period { get; init; } = default!;
    public ReservationStatus Status { get; init; } = default!;
}
