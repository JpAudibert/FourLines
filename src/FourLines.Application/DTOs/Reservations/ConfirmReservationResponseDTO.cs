namespace FourLines.Application.DTOs.Reservations;

public class ConfirmReservationResponseDTO
{
    public Reservation Reservation { get; init; } = default!;
    public Match Match { get; init; } = default!;
}
