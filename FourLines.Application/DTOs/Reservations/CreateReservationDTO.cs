using FourLines.Application.DTOs.Reservations.Interfaces;

namespace FourLines.Application.DTOs.Reservations;

public record CreateReservationDTO : ICreateReservationDTO
{
    public Guid CourtId { get; init; }
    public Guid UserId { get; init; }
    public TimeRange Period { get; init; } = default!;
    public ReservationStatus Status { get; init; } = default!;
}
