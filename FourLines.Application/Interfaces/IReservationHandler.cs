using FourLines.Application.DTOs.Reservations.Interfaces;

namespace FourLines.Application.Interfaces;

public interface IReservationHandler
{
    Task<Result<ConfirmReservationResponseDTO>> Create(
        ICreateReservationDTO newReservation,
        CancellationToken cancellationToken = default
    );
    Task<Result<Reservation>> UpdateReservationStatus(
        IUpdateStatusFromReservationDTO reservation,
        CancellationToken cancellationToken = default
    );
    Task<Result<bool>> Delete(
        IDeleteReservationDTO deleteDto,
        CancellationToken cancellationToken = default
    );
    Task<Result<IEnumerable<Reservation>>> GetAllReservationsFromUser(
        Guid userId,
        CancellationToken cancellationToken = default
    );
    Task<Result<IEnumerable<Reservation>>> GetAllReservationsFromCourt(
        Guid courtId,
        CancellationToken cancellationToken = default
    );
    Task<Result<Reservation>> GetOneReservationFromUser(
        Guid userId,
        Guid reservationId,
        CancellationToken cancellationToken = default
    );
}
