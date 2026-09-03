namespace FourLines.Application.Validators;

public class ReservationValidator() : IReservationValidator
{
    public async Task<Result<ConfirmReservationResponseDTO>> ValidateAsync(
        CreateReservationDTO reservation,
        CancellationToken cancellationToken = default
    )
    {
        if (!reservation.Period.AreDatesValid())
            return Result<ConfirmReservationResponseDTO>.Failure(ReservationsErrorResults.CreationInvalidDates);

        if (reservation.Period.StartAndEndAreInThePast())
            return Result<ConfirmReservationResponseDTO>.Failure(
                ReservationsErrorResults.CreationStartAndEndInThePast
            );

        if (!reservation.Period.StartAndEndAreInTheSameDay())
            return Result<ConfirmReservationResponseDTO>.Failure(
                ReservationsErrorResults.CreationStartAndEndNotInTheSameDay
            );

        if (reservation.Period.Duration != TimeSpan.FromMinutes(60))
            return Result<ConfirmReservationResponseDTO>.Failure(
                ReservationsErrorResults.CreationDurationTimeDifferentThanConfiguration
            );

        ReservationStatus[] statuses = Enum.GetValues<ReservationStatus>();
        if (!statuses.Contains(reservation.Status))
            return Result<ConfirmReservationResponseDTO>.Failure(ReservationsErrorResults.CreationInvalidStatus);

        return Result<ConfirmReservationResponseDTO>.Success(
            new ConfirmReservationResponseDTO
            {
                Match = default!,
                Reservation = new Reservation()
                {
                    CourtId = reservation.CourtId,
                    UserId = reservation.UserId,
                    Period = reservation.Period,
                    Status = reservation.Status,
                },
            }
        );
    }
}
