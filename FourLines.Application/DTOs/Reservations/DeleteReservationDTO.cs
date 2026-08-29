namespace FourLines.Application.DTOs.Reservations;

public class DeleteReservationDTO
{
    public Guid UserId { get; set; }
    public Guid ReservationId { get; set; }
}
