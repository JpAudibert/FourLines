using FourLines.Application.DTOs.Reservations.Interfaces;

namespace FourLines.Application.Interfaces;

public interface IReservationHandler
{
    Task<Result<ConfirmReservationResponseDTO>> Create(ICreateReservationDTO newReservation);
    Task<Result<Reservation>> UpdateReservationStatus(IUpdateStatusFromReservationDTO reservation);
    Task<Result<bool>> Delete(IDeleteReservationDTO deleteDto);
    Task<Result<IEnumerable<Reservation>>> GetAllReservationsFromUser(Guid userId);
    Task<Result<IEnumerable<Reservation>>> GetAllReservationsFromCourt(Guid courtId);
    Task<Result<Reservation>> GetOneReservationFromUser(Guid userId, Guid reservationId);
}
