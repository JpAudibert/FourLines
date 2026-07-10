namespace FourLines.Application.DTOs.Reservations;

public class UpdateStatusFromReservationDTO
{
    public Guid Id { get; init; } = default!;
    public Guid UserId { get; init; } = default!;
    public ReservationStatus Status { get; init; } = default!;
}
