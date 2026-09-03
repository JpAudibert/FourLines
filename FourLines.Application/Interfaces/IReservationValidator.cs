namespace FourLines.Application.Interfaces;

public interface IReservationValidator
{
    Task<Result<ConfirmReservationResponseDTO>> ValidateAsync(CreateReservationDTO reservationDTO, CancellationToken cancellationToken = default);
}
