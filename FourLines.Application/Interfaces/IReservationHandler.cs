namespace FourLines.Application.Interfaces;

public interface IReservationHandler
{
    Task<Result<ConfirmReservationResponseDTO>> Create(CreateReservationDTO newReservation);
    Task<Result<Reservation>> UpdateReservationStatus(UpdateStatusFromReservationDTO reservation);
    Task<Result<bool>> Delete(DeleteReservationDTO deleteDto);
    Task<Result<IEnumerable<Reservation>>> GetAllReservationsFromUser(Guid userId);
    Task<Result<IEnumerable<Reservation>>> GetAllReservationsFromCourt(Guid courtId);
    Task<Result<Reservation>> GetOneReservationFromUser(Guid userId, Guid reservationId);
}
