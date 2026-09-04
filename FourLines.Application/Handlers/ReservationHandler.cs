namespace FourLines.Application.Handlers;

public class ReservationHandler(
    FourLinesContext context, 
    IReservationValidator reservationValidator, 
    ICourtLockStrategies courtLockStrategy) 
    : IReservationHandler
{

    private const string DefaultMatchName = "World Cup Match";

    public async Task<Result<ConfirmReservationResponseDTO>> Create(CreateReservationDTO newReservation)
    {
        Result<ConfirmReservationResponseDTO> validationResult = await reservationValidator.ValidateAsync(newReservation);
        if (validationResult.IsFailure)
            return validationResult;

        using IDbContextTransaction transaction = await context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        Court? court = await courtLockStrategy.GetForUpdateAsync(newReservation.CourtId);
        if (court is null)
            return Result<ConfirmReservationResponseDTO>.Failure(ReservationsErrorResults.CreationUnknownCourt);

        User? user = await context.Users.FirstOrDefaultAsync(u => u.Id == newReservation.UserId);
        if (user is null)
            return Result<ConfirmReservationResponseDTO>.Failure(ReservationsErrorResults.CreationUnknownUser);

        DayOfWeek dayOfWeek = newReservation.Period.Start.DayOfWeek;
        TimeOnly reservationStartTime = TimeOnly.FromDateTime(newReservation.Period.Start.DateTime);
        TimeOnly reservationEndTime = TimeOnly.FromDateTime(newReservation.Period.End.DateTime);

        FacilitySchedule? schedule = await context.FacilitySchedules
            .FirstOrDefaultAsync(s => s.FacilityId == court.FacilityId &&
                s.DayOfWeek == dayOfWeek &&
                s.OpensAt <= reservationStartTime &&
                s.ClosesAt >= reservationEndTime);

        if (schedule is null)
            return Result<ConfirmReservationResponseDTO>.Failure(ReservationsErrorResults.CreationOutsideFacilitySchedule);

        Reservation reservation = new()
        {
            CourtId = newReservation.CourtId,
            UserId = newReservation.UserId,
            Period = newReservation.Period,
            Status = ReservationStatus.Pending,
            Court = court,
            User = user
        };

        bool overlappingReservation = await context.Reservations
            .Where(r => 
                r.CourtId == newReservation.CourtId &&
                r.Period.Start < newReservation.Period.End &&
                r.Period.End > newReservation.Period.Start &&
                r.Status != ReservationStatus.Cancelled)
            .AnyAsync();

        if (overlappingReservation)
            return Result<ConfirmReservationResponseDTO>.Failure(ReservationsErrorResults.CreationOverlappingReservation);

        Match newMatch = new()
        {
            ReservationId = reservation.Id,
            SportId = court.SportId,
            Name = DefaultMatchName,
            Code = Random.Shared.Next(0, 1000000).ToString("D6"),
            Reservation = reservation,
            Sport = court.Sport
        };

        await context.Reservations.AddAsync(reservation);
        await context.Matches.AddAsync(newMatch);

        await context.SaveChangesAsync();

        await transaction.CommitAsync();

        return Result<ConfirmReservationResponseDTO>.Success(new ConfirmReservationResponseDTO
        {
            Reservation = reservation,
            Match = newMatch
        });
    }

    public async Task<Result<Reservation>> UpdateReservationStatus(UpdateStatusFromReservationDTO reservation)
    {
        ReservationStatus[] statuses = Enum.GetValues<ReservationStatus>();
        if (!statuses.Contains(reservation.Status))
            return Result<Reservation>.Failure(ReservationsErrorResults.CreationInvalidStatus);

        int affectedRows = await context.Reservations
            .Where(r => r.Id == reservation.Id && r.UserId == reservation.UserId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(r => r.Status, reservation.Status)
            );

        if (affectedRows <= 0)
            return Result<Reservation>.Failure(ReservationsErrorResults.UpdateReservationDoesNotExist);

        await context.SaveChangesAsync();

        Reservation? updatedReservation = await context.Reservations.FindAsync(reservation.Id);

        return Result<Reservation>.Success(updatedReservation!);
    }

    public async Task<Result<bool>> Delete(DeleteReservationDTO deleteDto)
    {
        int affectedRows = await context.Reservations
            .Where(r => r.Id == deleteDto.ReservationId && r.UserId == deleteDto.UserId)
            .ExecuteDeleteAsync();

        if (affectedRows <= 0)
            return Result<bool>.Failure(ReservationsErrorResults.DeletionReservationDoesNotExist);

        await context.SaveChangesAsync();

        return Result<bool>.Success(true);
    }

    public async Task<Result<IEnumerable<Reservation>>> GetAllReservationsFromUser(Guid userId)
    {
        IEnumerable<Reservation> reservations = await context.Reservations
            .Where(r => r.UserId == userId)
            .Select(r => new Reservation
            {
                Id = r.Id,
                CourtId = r.CourtId,
                UserId = r.UserId,
                Period = r.Period,
                Status = r.Status
            })
            .ToListAsync();

        if (!reservations.Any())
            return Result<IEnumerable<Reservation>>.Failure(ReservationsErrorResults.GetAllNoReservationsForUser);

        return Result<IEnumerable<Reservation>>.Success(reservations);
    }

    public async Task<Result<IEnumerable<Reservation>>> GetAllReservationsFromCourt(Guid courtId)
    {
        IEnumerable<Reservation> reservations = await context.Reservations
            .Where(r => r.CourtId == courtId)
            .Select(r => new Reservation
            {
                Id = r.Id,
                CourtId = r.CourtId,
                UserId = r.UserId,
                Period = r.Period,
                Status = r.Status
            })
            .ToListAsync();

        if (!reservations.Any())
            return Result<IEnumerable<Reservation>>.Failure(ReservationsErrorResults.GetAllNoReservationsForCourt);

        return Result<IEnumerable<Reservation>>.Success(reservations);
    }

    public async Task<Result<Reservation>> GetOneReservationFromUser(Guid userId, Guid reservationId)
    {
        Reservation? reservation = await context.Reservations
            .FirstOrDefaultAsync(r => r.Id == reservationId && r.UserId == userId);

        if (reservation is null)
            return Result<Reservation>.Failure(ReservationsErrorResults.GetOneReservationDoesNotExist);

        return Result<Reservation>.Success(reservation);
    }
}
