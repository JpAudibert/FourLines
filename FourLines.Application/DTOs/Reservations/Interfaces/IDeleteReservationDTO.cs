namespace FourLines.Application.DTOs.Reservations.Interfaces
{
    public interface IDeleteReservationDTO
    {
        Guid ReservationId { get; init; }
        Guid UserId { get; init; }
    }
}