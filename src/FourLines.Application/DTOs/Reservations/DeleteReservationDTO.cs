namespace FourLines.Application.DTOs.Reservations;

public record DeleteReservationDTO
{
    public Guid UserId { get; init; }
    public Guid ReservationId { get; init; }
}
