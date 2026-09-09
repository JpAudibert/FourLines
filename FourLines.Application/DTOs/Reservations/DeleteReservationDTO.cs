using FourLines.Application.DTOs.Reservations.Interfaces;

namespace FourLines.Application.DTOs.Reservations;

public record DeleteReservationDTO : IDeleteReservationDTO
{
    public Guid UserId { get; init; }
    public Guid ReservationId { get; init; }
}
