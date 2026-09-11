using FourLines.Application.DTOs.Reservations.Interfaces;

namespace FourLines.Application.Interfaces;

public interface IReservationValidator
{
    Task<Result<ConfirmReservationResponseDTO>> ValidateAsync(ICreateReservationDTO reservationDTO, CancellationToken cancellationToken = default);
}
