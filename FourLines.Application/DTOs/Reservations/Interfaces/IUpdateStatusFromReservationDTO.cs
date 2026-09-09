namespace FourLines.Application.DTOs.Reservations.Interfaces
{
    public interface IUpdateStatusFromReservationDTO
    {
        Guid Id { get; init; }
        ReservationStatus Status { get; init; }
        Guid UserId { get; init; }
    }
}