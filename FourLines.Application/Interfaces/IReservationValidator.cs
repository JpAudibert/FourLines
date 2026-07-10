namespace FourLines.Application.Interfaces;

public interface IReservationValidator
{
    Task<Result<Reservation>> ValidateAsync(CreateReservationDTO reservationDTO, CancellationToken cancellationToken = default);
}
