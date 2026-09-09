using FourLines.Application.DTOs.Reservations.Interfaces;

namespace FourLines.Application.DTOs.Reservations;

public record UpdateStatusFromReservationDTO : IUpdateStatusFromReservationDTO
{
    public Guid Id { get; init; } = default!;
    public Guid UserId { get; init; } = default!;
    public ReservationStatus Status { get; init; } = default!;
}
