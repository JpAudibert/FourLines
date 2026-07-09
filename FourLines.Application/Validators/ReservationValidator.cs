namespace FourLines.Application.Validators;

public class ReservationValidator(FourLinesContext context) : IReservationValidator
{
    private readonly FourLinesContext _context = context;

    public async Task<Result<Reservation>> ValidateAsync(CreateReservationDTO reservation, CancellationToken cancellationToken = default)
    {
        if (!reservation.Period.AreDatesValid())
            return Result<Reservation>.Failure(ReservationsErrorResults.CreationInvalidDates);

        if (reservation.Period.Duration <= TimeSpan.FromMinutes(0))
            return Result<Reservation>.Failure(ReservationsErrorResults.CreationInvalidDuration);

        if (reservation.Period.StartAndEndAreInThePast())
            return Result<Reservation>.Failure(ReservationsErrorResults.CreationStartAndEndInThePast);

        if (!reservation.Period.StartAndEndAreInTheSameDay())
            return Result<Reservation>.Failure(ReservationsErrorResults.CreationStartAndEndNotInTheSameDay);

        if (reservation.Period.Duration != TimeSpan.FromMinutes(60))
            return Result<Reservation>.Failure(ReservationsErrorResults.CreationDurationTimeDifferentThanConfiguration);

        return Result<Reservation>.Success(new Reservation
        {
            CourtId = reservation.CourtId,
            UserId = reservation.UserId,
            Period = reservation.Period,
        });
    }
}
