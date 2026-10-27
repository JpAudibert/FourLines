namespace FourLines.Api.Models.RequestModels.Reservations;

public class CreateReservationRequest
{
    public Guid CourtId { get; init; }
    public TimeRange Period { get; init; } = default!;
    public ReservationStatus Status { get; init; } = default!;
}
