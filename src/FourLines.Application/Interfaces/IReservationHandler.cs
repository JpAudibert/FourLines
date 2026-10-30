namespace FourLines.Application.Interfaces;

public interface IReservationHandler
{
    Task<Result<ConfirmReservationResponseDTO>> Create(
        CreateReservationDTO newReservation,
        CancellationToken cancellationToken = default
    );
    Task<Result<Reservation>> UpdateReservationStatus(
        UpdateStatusFromReservationDTO reservation,
        CancellationToken cancellationToken = default
    );
    Task<Result<bool>> Delete(
        DeleteReservationDTO deleteDto,
        CancellationToken cancellationToken = default
    );
    Task<Result<IEnumerable<Reservation>>> GetAllReservationsFromUser(
        Guid userId,
        CancellationToken cancellationToken = default
    );
    Task<Result<IEnumerable<Reservation>>> GetAllReservationsFromCourt(
        Guid courtId,
        Guid ownerId,
        bool isAdmin,
        CancellationToken cancellationToken = default
    );
    Task<Result<Reservation>> GetOneReservationFromUser(
        Guid userId,
        Guid reservationId,
        CancellationToken cancellationToken = default
    );

    Task<Result<CourtAvailability>> GetAvailableTimesToPlayInCourt(
        Guid courtId,
        DateOnly selectedDate,
        CancellationToken cancellationToken = default
    );
}
