using FourLines.Application.Validators.Interfaces;

namespace FourLines.Application.Handlers;

public class ReservationHandler(
    FourLinesContext context,
    IReservationValidator reservationValidator,
    ICourtLockStrategies courtLockStrategy,
    IAvailabilityHandler availabilityHandler,
    TimeZoneValidator timeZoneValidator
) : IReservationHandler
{
    private const string DefaultMatchName = "World Cup Match";

    public async Task<Result<ConfirmReservationResponseDTO>> Create(
        CreateReservationDTO newReservation,
        CancellationToken cancellationToken = default
    )
    {
        Result<ConfirmReservationResponseDTO> validationResult =
            await reservationValidator.ValidateAsync(newReservation, cancellationToken);
        if (validationResult.IsFailure)
            return validationResult;

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(
            IsolationLevel.ReadCommitted,
            cancellationToken: cancellationToken
        );

        Court? court = await courtLockStrategy.GetForUpdateAsync(
            newReservation.CourtId,
            cancellationToken
        );
        if (court is null)
            return Result<ConfirmReservationResponseDTO>.Failure(
                ReservationsErrorResults.CreationUnknownCourt
            );

        User? user = await context.Users.FirstOrDefaultAsync(
            u => u.Id == newReservation.UserId,
            cancellationToken: cancellationToken
        );
        if (user is null)
            return Result<ConfirmReservationResponseDTO>.Failure(
                ReservationsErrorResults.CreationUnknownUser
            );

        DayOfWeek dayOfWeek = newReservation.Period.Start.DayOfWeek;
        TimeOnly reservationStartTime = TimeOnly.FromDateTime(newReservation.Period.Start.DateTime);
        TimeOnly reservationEndTime = TimeOnly.FromDateTime(newReservation.Period.End.DateTime);

        FacilitySchedule? schedule = await context
            .FacilitySchedules.Include(fs => fs.Facility)
            .FirstOrDefaultAsync(
                s =>
                    s.FacilityId == court.FacilityId
                    && s.DayOfWeek == dayOfWeek
                    && s.OpensAt <= reservationStartTime
                    && s.ClosesAt >= reservationEndTime,
                cancellationToken: cancellationToken
            );

        if (schedule is null)
            return Result<ConfirmReservationResponseDTO>.Failure(
                ReservationsErrorResults.CreationOutsideFacilitySchedule
            );

        if (!timeZoneValidator.Validate(schedule.Facility.TimeZoneId).IsValid)
            return Result<ConfirmReservationResponseDTO>.Failure(
                ReservationsErrorResults.InvalidTimeZone
            );

        TimeRange utcReservationPeriod = TimeZoneProvider.EnsureUtcForTimeRange(
            schedule.Facility.TimeZoneId,
            newReservation.Period
        );

        Reservation reservation = new()
        {
            CourtId = newReservation.CourtId,
            UserId = newReservation.UserId,
            Period = utcReservationPeriod,
            Status = ReservationStatus.Pending,
            Price = newReservation.Price,
            Court = court,
            User = user,
        };

        bool overlappingReservation = await context
            .Reservations.Where(r =>
                r.CourtId == newReservation.CourtId
                && r.Period.Start < utcReservationPeriod.End
                && r.Period.End > utcReservationPeriod.Start
                && r.Status != ReservationStatus.Cancelled
            )
            .AnyAsync(cancellationToken: cancellationToken);

        if (overlappingReservation)
            return Result<ConfirmReservationResponseDTO>.Failure(
                ReservationsErrorResults.CreationOverlappingReservation
            );

        Match newMatch = new()
        {
            ReservationId = reservation.Id,
            SportId = court.SportId,
            Name = DefaultMatchName,
            Code = Match.GenerateMatchCode(),
            Reservation = reservation,
            Sport = court.Sport,
        };

        await context.Reservations.AddAsync(reservation, cancellationToken);
        await context.Matches.AddAsync(newMatch, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return Result<ConfirmReservationResponseDTO>.Success(
            new ConfirmReservationResponseDTO { Reservation = reservation, Match = newMatch }
        );
    }

    public async Task<Result<Reservation>> UpdateReservationStatus(
        UpdateStatusFromReservationDTO reservation,
        CancellationToken cancellationToken = default
    )
    {
        ReservationStatus[] statuses = Enum.GetValues<ReservationStatus>();
        if (!statuses.Contains(reservation.Status))
            return Result<Reservation>.Failure(ReservationsErrorResults.CreationInvalidStatus);

        int affectedRows = await context
            .Reservations.Where(r => r.Id == reservation.Id && r.UserId == reservation.UserId)
            .ExecuteUpdateAsync(
                setters => setters.SetProperty(r => r.Status, reservation.Status),
                cancellationToken: cancellationToken
            );

        if (affectedRows <= 0)
            return Result<Reservation>.Failure(
                ReservationsErrorResults.UpdateReservationDoesNotExist
            );

        Reservation? updatedReservation = await context.Reservations.FindAsync(
            new object?[] { reservation.Id },
            cancellationToken: cancellationToken
        );

        return Result<Reservation>.Success(updatedReservation!);
    }

    public async Task<Result<bool>> Delete(
        DeleteReservationDTO deleteDto,
        CancellationToken cancellationToken = default
    )
    {
        int affectedRows = await context
            .Reservations.Where(r =>
                r.Id == deleteDto.ReservationId && r.UserId == deleteDto.UserId
            )
            .ExecuteDeleteAsync(cancellationToken: cancellationToken);

        if (affectedRows <= 0)
            return Result<bool>.Failure(ReservationsErrorResults.DeletionReservationDoesNotExist);

        await context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }

    public async Task<Result<IEnumerable<Reservation>>> GetAllReservationsFromUser(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        IEnumerable<Reservation> reservations = await context
            .Reservations.Where(r => r.UserId == userId)
            .Select(r => new Reservation
            {
                Id = r.Id,
                CourtId = r.CourtId,
                UserId = r.UserId,
                Period = r.Period,
                Status = r.Status,
            })
            .ToListAsync(cancellationToken: cancellationToken);

        if (!reservations.Any())
            return Result<IEnumerable<Reservation>>.Failure(
                ReservationsErrorResults.GetAllNoReservationsForUser
            );

        return Result<IEnumerable<Reservation>>.Success(reservations);
    }

    public async Task<Result<IEnumerable<Reservation>>> GetAllReservationsFromCourt(
        Guid courtId,
        CancellationToken cancellationToken = default
    )
    {
        IEnumerable<Reservation> reservations = await context
            .Reservations.Where(r => r.CourtId == courtId)
            .Select(r => new Reservation
            {
                Id = r.Id,
                CourtId = r.CourtId,
                UserId = r.UserId,
                Period = r.Period,
                Status = r.Status,
                Price = r.Price,
            })
            .ToListAsync(cancellationToken: cancellationToken);

        if (!reservations.Any())
            return Result<IEnumerable<Reservation>>.Failure(
                ReservationsErrorResults.GetAllNoReservationsForCourt
            );

        return Result<IEnumerable<Reservation>>.Success(reservations);
    }

    public async Task<Result<Reservation>> GetOneReservationFromUser(
        Guid userId,
        Guid reservationId,
        CancellationToken cancellationToken = default
    )
    {
        Reservation? reservation = await context.Reservations.FirstOrDefaultAsync(
            r => r.Id == reservationId && r.UserId == userId,
            cancellationToken: cancellationToken
        );

        if (reservation is null)
            return Result<Reservation>.Failure(
                ReservationsErrorResults.GetOneReservationDoesNotExist
            );

        return Result<Reservation>.Success(reservation);
    }

    public async Task<Result<CourtAvailability>> GetAvailableTimesToPlayInCourt(
        Guid courtId,
        DateOnly selectedDate,
        CancellationToken cancellationToken = default
    )
    {
        FacilityTimeSlot facilityTimeSlots = await availabilityHandler.GetTimeSlots(
            courtId,
            selectedDate.DayOfWeek,
            cancellationToken
        );

        DateTimeOffset now = TimeZoneProvider.ToFacilityTime(
            facilityTimeSlots.TimeZoneId,
            DateTimeOffset.UtcNow
        );

        IQueryable<Reservation> reservationsQuery = context
            .Reservations.Where(r =>
                r.CourtId == courtId
                && DateOnly.FromDateTime(r.Period.Start.DateTime) == selectedDate
                && r.Status != ReservationStatus.Cancelled
            )
            .Select(r => new Reservation { Period = r.Period });
        List<TimeSlot> availableSlots = [];

        foreach (TimeSlot slot in facilityTimeSlots.TimeSlots)
        {
            bool isPast = slot.Start <= TimeOnly.FromDateTime(now.DateTime);
            bool hasReservation = false;

            if (!isPast)
            {
                hasReservation = reservationsQuery.Any(r =>
                    r.Period.Start.TimeOfDay < slot.Start.ToTimeSpan()
                    && r.Period.End.TimeOfDay > slot.End.ToTimeSpan()
                );
            }

            availableSlots.Add(
                new TimeSlot()
                {
                    Start = slot.Start,
                    End = slot.End,
                    Price = slot.Price,
                    Available = !hasReservation && !isPast,
                }
            );
        }

        CourtAvailability availability = new()
        {
            CourtId = courtId,
            PlayingDate = selectedDate,
            TimeSlots = availableSlots,
        };

        return Result<CourtAvailability>.Success(availability);
    }
}
