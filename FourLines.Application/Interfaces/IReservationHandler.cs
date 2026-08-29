namespace FourLines.Application.Interfaces;

public interface IReservationHandler : ICrudHandler<Reservation, CreateReservationDTO, UpdateStatusFromReservationDTO, DeleteReservationDTO>
{
    Task<Result<IEnumerable<Reservation>>> GetAllReservationsFromUser(Guid userId);
    Task<Result<IEnumerable<Reservation>>> GetAllReservationsFromCourt(Guid courtId);
    Task<Result<Reservation>> GetOneReservationFromUser(Guid userId, Guid reservationId);
}
