using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace FourLines.Application.Handlers;

public class ReservationHandler(FourLinesContext context, IReservationValidator reservationValidator)
{
    private readonly FourLinesContext _context = context;
    private readonly IReservationValidator _reservationValidator = reservationValidator;

    public async Task<Result<Reservation>> Create(CreateReservationDTO newReservation)
    {
        Result<Reservation> validationResult = await _reservationValidator.ValidateAsync(newReservation);

        if (validationResult.IsFailure)
            return validationResult;

        using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        Court? court = await _context.Courts.FirstOrDefaultAsync(c => c.Id == newReservation.CourtId);
        if (court is null)
            return Result<Reservation>.Failure(ReservationsErrorResults.CreationUnknownCourt);

        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == newReservation.UserId);
        if (user is null)
            return Result<Reservation>.Failure(ReservationsErrorResults.CreationUnknownUser);

        DayOfWeek dayOfWeek = newReservation.Period.Start.DayOfWeek;
        TimeOnly reservationStartTime = TimeOnly.FromDateTime(newReservation.Period.Start.DateTime);
        TimeOnly reservationEndTime = TimeOnly.FromDateTime(newReservation.Period.End.DateTime);

        FacilitySchedule? schedule = await _context.FacilitySchedules
            .FirstOrDefaultAsync(s => s.FacilityId == court.FacilityId &&
                s.DayOfWeek == dayOfWeek &&
                s.OpensAt <= reservationStartTime &&
                s.ClosesAt >= reservationEndTime);

        if (schedule is null)
            return Result<Reservation>.Failure(ReservationsErrorResults.CreationOutsideFacilitySchedule);

        Reservation reservation = new()
        {
            CourtId = newReservation.CourtId,
            UserId = newReservation.UserId,
            Period = newReservation.Period,
            Status = ReservationStatus.Pending,
            Court = court,
            User = user
        };

        bool overlappingReservation = await _context.Reservations
            .AnyAsync(r => r.CourtId == newReservation.CourtId &&
                r.Period.Start < newReservation.Period.End &&
                r.Period.End > newReservation.Period.Start &&
                r.Status != ReservationStatus.Cancelled);

        if (overlappingReservation)
            return Result<Reservation>.Failure(ReservationsErrorResults.CreationOverlappingReservation);

        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();

        return Result<Reservation>.Success(reservation);
    }

    public async Task<Result<Reservation>> UpdateStatusFromReservation(UpdateStatusFromReservationDTO reservation)
    {
        int affectedRows = await _context.Reservations
            .Where(r => r.Id == reservation.Id && r.UserId == reservation.UserId)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(r => r.Status, reservation.Status)
            );

        if (affectedRows <= 0)
            return Result<Reservation>.Failure(ReservationsErrorResults.UpdateReservationDoesNotExist);

        await _context.SaveChangesAsync();

        Reservation? updatedReservation = await _context.Reservations.FindAsync(reservation.Id);

        return Result<Reservation>.Success(updatedReservation!);
    }

    public async Task<Result<bool>> Delete(Guid userId, Guid reservationId)
    {
        int affectedRows = await _context.Reservations
            .Where(r => r.Id == reservationId && r.UserId == userId)
            .ExecuteDeleteAsync();

        if (affectedRows <= 0)
            return Result<bool>.Failure(ReservationsErrorResults.DeletionReservationDoesNotExist);

        await _context.SaveChangesAsync();

        return Result<bool>.Success(true);
    }

    public async Task<Result<IEnumerable<Reservation>>> GetAllReservationsFromUser(Guid userId)
    {
        IEnumerable<Reservation> reservations = await _context.Reservations
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
        IEnumerable<Reservation> reservations = await _context.Reservations
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
        Reservation? reservation = await _context.Reservations
            .FirstOrDefaultAsync(r => r.Id == reservationId && r.UserId == userId);

        if (reservation is null)
            return Result<Reservation>.Failure(ReservationsErrorResults.GetOneReservationDoesNotExist);

        return Result<Reservation>.Success(reservation);
    }
}
