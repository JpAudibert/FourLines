namespace FourLines.Api.Models.RequestModels.Reservations;

public class UpdateReservationStatusRequest
{
    public ReservationStatus Status { get; init; } = default!;
}
