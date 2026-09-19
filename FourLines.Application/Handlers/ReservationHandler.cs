using FourLines.Application.DTOs.Reservations.Interfaces;

namespace FourLines.Application.Handlers;

public class ReservationHandler(
    FourLinesContext context,
    IReservationValidator reservationValidator,
    ICourtLockStrategies courtLockStrategy
) : IReservationHandler
{
    private const string DefaultMatchName = "World Cup Match";

    public async Task<Result<ConfirmReservationResponseDTO>> Create(
        ICreateReservationDTO newReservation,
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

        FacilitySchedule? schedule = await context.FacilitySchedules.FirstOrDefaultAsync(
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

        Reservation reservation = new()
        {
            CourtId = newReservation.CourtId,
            UserId = newReservation.UserId,
            Period = newReservation.Period,
            Status = ReservationStatus.Pending,
            Price = newReservation.Price,
            Court = court,
            User = user,
        };

        bool overlappingReservation = await context
            .Reservations.Where(r =>
                r.CourtId == newReservation.CourtId
                && r.Period.Start < newReservation.Period.End
                && r.Period.End > newReservation.Period.Start
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
        IUpdateStatusFromReservationDTO reservation,
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
        IDeleteReservationDTO deleteDto,
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
}
