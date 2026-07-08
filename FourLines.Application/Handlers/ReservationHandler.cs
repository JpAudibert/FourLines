using FourLines.Application.DTOs.Reservations;

namespace FourLines.Application.Handlers;

public class ReservationHandler(FourLinesContext context)
{
    private readonly FourLinesContext _context = context;

    public async Task<Result<Reservation>> Create(CreateReservationDTO newReservation)
    {
        Court? court = await _context.Courts.FirstOrDefaultAsync(c => c.Id == newReservation.CourtId);
        if (court is null)
            return Result<Reservation>.Failure(ReservationsErrorResults.CreationUnknownCourt);

        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Id == newReservation.UserId);
        if (user is null)
            return Result<Reservation>.Failure(ReservationsErrorResults.CreationUnknownUser);

        Reservation reservation = new()
        {
            CourtId = newReservation.CourtId,
            UserId = newReservation.UserId,
            Period = newReservation.Period,
            Status = ReservationStatus.Pending
        };

        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();

        return Result<Reservation>.Success(reservation);
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

        if(!reservations.Any())
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
