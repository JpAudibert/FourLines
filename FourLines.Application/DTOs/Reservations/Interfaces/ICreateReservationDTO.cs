namespace FourLines.Application.DTOs.Reservations.Interfaces
{
    public interface ICreateReservationDTO
    {
        Guid CourtId { get; init; }
        TimeRange Period { get; init; }
        ReservationStatus Status { get; init; }
        Guid UserId { get; init; }
    }
}