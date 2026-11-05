namespace FourLines.Application.Handlers;

public sealed class CourtBookingPresentationHandler(
    FourLinesContext context,
    IReservationHandler reservationHandler
) : ICourtBookingPresentationHandler
{
    public async Task<IReadOnlyList<Facility>> GetHome(
        string? cityName,
        CancellationToken cancellationToken = default
    ) =>
        await context
            .Facilities.AsNoTracking()
            .Where(f => cityName == null || f.City == cityName)
            .Include(f => f.Courts)
            .ThenInclude(c => c.Sport)
            .ToListAsync(cancellationToken);

    public async Task<Result<int>> GetAvailableTimeSlotsForToday(
        Facility facility,
        DateTimeOffset utcNow,
        CancellationToken cancellationToken = default
    )
    {
        int availableTimeSlotsCount = 0;
        DateOnly facilityToday = DateOnly.FromDateTime(
            TimeZoneProvider.ToFacilityTime(facility.TimeZoneId, utcNow).DateTime
        );

        foreach (Court court in facility.Courts.Where(c => c.IsActive))
        {
            Result<CourtAvailability> availability =
                await reservationHandler.GetAvailableTimesToPlayInCourt(
                    court.Id,
                    facilityToday,
                    cancellationToken
                );

            if (availability.IsFailure)
                return Result<int>.Failure(availability.Error);

            availableTimeSlotsCount += availability.Value.TimeSlots.Count(slot => slot.Available);
        }

        return Result<int>.Success(availableTimeSlotsCount);
    }

    public async Task<Facility?> GetFacilityCourts(
        Guid facilityId,
        CancellationToken cancellationToken = default
    ) =>
        await context
            .Facilities.AsNoTracking()
            .Include(f => f.Courts)
            .ThenInclude(c => c.Sport)
            .FirstOrDefaultAsync(f => f.Id == facilityId, cancellationToken);

    public Task<Result<CourtAvailability>> GetCourtAvailability(
        Guid courtId,
        DateOnly selectedDate,
        CancellationToken cancellationToken = default
    ) =>
        reservationHandler.GetAvailableTimesToPlayInCourt(
            courtId,
            selectedDate,
            cancellationToken
        );

    public async Task<IReadOnlyList<Match>> GetMatchesForUser(
        Guid userId,
        int limit,
        CancellationToken cancellationToken = default
    ) =>
        await context
            .Matches.AsNoTracking()
            .Where(m => m.MatchesUsers.Any(mu => mu.UserId == userId))
            .OrderByDescending(m => m.Reservation.Period.Start)
            .Take(limit)
            .Include(m => m.Sport)
            .Include(m => m.Reservation)
            .ThenInclude(r => r.Court)
            .ThenInclude(c => c.Facility)
            .Include(m => m.MatchesUsers)
            .ThenInclude(mu => mu.User)
            .ToListAsync(cancellationToken);

    public async Task<Match?> GetMatch(
        Guid matchId,
        CancellationToken cancellationToken = default
    ) =>
        await context
            .Matches.AsNoTracking()
            .Where(m => m.Id == matchId)
            .Include(m => m.Sport)
            .Include(m => m.Reservation)
            .ThenInclude(r => r.Court)
            .ThenInclude(c => c.Facility)
            .Include(m => m.MatchesUsers)
            .ThenInclude(mu => mu.User)
            .FirstOrDefaultAsync(cancellationToken);
}
