namespace FourLines.Application.Interfaces;

public interface ICourtBookingPresentationHandler
{
    Task<IReadOnlyList<Facility>> GetHome(
        string? cityName,
        CancellationToken cancellationToken = default
    );

    Task<Result<int>> GetAvailableTimeSlotsForToday(
        Facility facility,
        DateTimeOffset utcNow,
        CancellationToken cancellationToken = default
    );

    Task<Facility?> GetFacilityCourts(
        Guid facilityId,
        CancellationToken cancellationToken = default
    );

    Task<Result<CourtAvailability>> GetCourtAvailability(
        Guid courtId,
        DateOnly selectedDate,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<Match>> GetMatchesForUser(
        Guid userId,
        int limit,
        CancellationToken cancellationToken = default
    );

    Task<Match?> GetMatch(Guid matchId, CancellationToken cancellationToken = default);
}
