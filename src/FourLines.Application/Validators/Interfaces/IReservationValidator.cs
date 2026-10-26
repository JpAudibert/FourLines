namespace FourLines.Application.Validators.Interfaces;

public interface IReservationValidator
{
    Task<Result<ConfirmReservationResponseDTO>> ValidateAsync(
        CreateReservationDTO reservationDTO,
        CancellationToken cancellationToken = default
    );
}
