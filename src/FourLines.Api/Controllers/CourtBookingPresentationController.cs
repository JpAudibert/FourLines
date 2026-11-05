using FourLines.Api.Models.Presentation.Response;
using FourLines.Application.Interfaces;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/presentation")]
public sealed class CourtBookingPresentationController(
    ILogger<CourtBookingPresentationController> logger,
    ICourtBookingPresentationHandler presentationHandler,
    ICurrentUserProvider currentUserProvider
) : ApiControllerBase(logger, currentUserProvider)
{
    private const string DefaultMatchName = "World Cup Match";

    /// <summary>Gets facility and court information for the home view.</summary>
    [HttpGet("home")]
    [EndpointSummary("Get home facilities")]
    [EndpointDescription("Returns facilities and their distinct sports, optionally filtered by city.")]
    [ProducesResponseType<HomeResponse>(StatusCodes.Status200OK)]
    public async Task<ActionResult<HomeResponse>> GetHome(
        [FromQuery] string? cityName,
        CancellationToken cancellationToken
    )
    {
        IReadOnlyList<Facility> facilities = await presentationHandler.GetHome(
            cityName,
            cancellationToken
        );

        DateTimeOffset utcNow = DateTimeOffset.UtcNow;
        List<FacilityListItemResponse> facilityItems = [];

        foreach (Facility facility in facilities)
        {
            Result<int> availableTimeSlots = await presentationHandler.GetAvailableTimeSlotsForToday(
                facility,
                utcNow,
                cancellationToken
            );

            if (availableTimeSlots.IsFailure)
            {
                logger.LogWarning(
                    "Could not retrieve today's available time slots for facility {FacilityId}: {ErrorCode} - {ErrorDescription}",
                    facility.Id,
                    availableTimeSlots.Error.Code,
                    availableTimeSlots.Error.Description
                );

                return Problem(
                    title: availableTimeSlots.Error.Code,
                    detail: availableTimeSlots.Error.Description,
                    statusCode: StatusCodes.Status404NotFound
                );
            }

            facilityItems.Add(
                new FacilityListItemResponse(
                    facility.Id,
                    facility.Name,
                    facility.Address,
                    [.. facility.Courts
                        .Select(c => new SportResponse(c.SportId, c.Sport.Name))
                        .DistinctBy(sport => sport.SportId)],
                    availableTimeSlots.Value
                )
            );
        }

        return Ok(new HomeResponse(facilityItems));
    }

    /// <summary>Gets facility details and its courts.</summary>
    [HttpGet("facilities/{facilityId:guid}/courts")]
    [EndpointSummary("Get facility courts")]
    [EndpointDescription("Returns facility details and court sport and default price information.")]
    [ProducesResponseType<FacilityCourtsResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<FacilityCourtsResponse>> GetFacilityCourts(
        [FromRoute] Guid facilityId,
        CancellationToken cancellationToken
    )
    {
        Facility? facility = await presentationHandler.GetFacilityCourts(
            facilityId,
            cancellationToken
        );
        if (facility is null)
            return NotFound();

        CourtListItemResponse[] courts = [.. facility.Courts
            .Select(c =>
                new CourtListItemResponse(
                    c.Id,
                    c.Name,
                    c.SportId,
                    c.Sport.Name,
                    c.RentingPeriodInMinutes + c.MaintenancePeriodInMinutes,
                    new MoneyResponse(c.DefaultPrice.Amount, c.DefaultPrice.Currency)
                )
            )];

        return Ok(
            new FacilityCourtsResponse(facility.Id, facility.Name, facility.Address, courts)
        );
    }

    /// <summary>Gets court availability for a selected date.</summary>
    [HttpGet("courts/{courtId:guid}/selected-date/{selectedDate}")]
    [EndpointSummary("Get court availability by date")]
    [EndpointDescription("Returns time slots for the requested date, including price and availability.")]
    [ProducesResponseType<CourtAvailabilityResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourtAvailabilityResponse>> GetCourtAvailability(
        [FromRoute] Guid courtId,
        [FromRoute] DateOnly selectedDate,
        CancellationToken cancellationToken
    )
    {
        Result<CourtAvailability> result = await presentationHandler.GetCourtAvailability(
            courtId,
            selectedDate,
            cancellationToken
        );

        if (result.IsFailure)
        {
            logger.LogWarning(
                "Could not retrieve availability for court {CourtId}: {ErrorCode} - {ErrorDescription}",
                courtId,
                result.Error.Code,
                result.Error.Description
            );

            return Problem(
                title: result.Error.Code,
                detail: result.Error.Description,
                statusCode: StatusCodes.Status404NotFound
            );
        }

        TimeSlotResponse[] timeSlots = [.. result
            .Value.TimeSlots.Select(slot =>
                new TimeSlotResponse(
                    slot.Start,
                    slot.End,
                    new MoneyResponse(slot.Price.Amount, slot.Price.Currency),
                    slot.Available
                )
            )];

        return Ok(
            new CourtAvailabilityResponse(
                result.Value.CourtId,
                result.Value.PlayingDate,
                timeSlots
            )
        );
    }

    /// <summary>Gets the authenticated user's most recent matches.</summary>
    [HttpGet("matches/me")]
    [EndpointSummary("Get the current user's matches")]
    [EndpointDescription("Returns the authenticated user's matches, most recent first.")]
    [ProducesResponseType<IReadOnlyList<MatchListItemResponse>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IReadOnlyList<MatchListItemResponse>>> GetMyMatches(
        [FromQuery, Range(1, int.MaxValue)] int limit = 5,
        CancellationToken cancellationToken = default
    )
    {
        if (!TryGetCurrentUserId(out Guid userId))
            return Unauthorized();

        IReadOnlyList<Match> matches = await presentationHandler.GetMatchesForUser(
            userId,
            limit,
            cancellationToken
        );

        MatchListItemResponse[] response = [.. matches.Select(ToMatchListItem)];
        return Ok(response);
    }

    /// <summary>Gets the details of a match.</summary>
    [HttpGet("matches/{matchId:guid}")]
    [EndpointSummary("Get match details")]
    [EndpointDescription("Returns match, court, facility, sport, and player details.")]
    [ProducesResponseType<MatchDetailResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MatchDetailResponse>> GetMatch(
        [FromRoute] Guid matchId,
        CancellationToken cancellationToken
    )
    {
        Match? match = await presentationHandler.GetMatch(matchId, cancellationToken);
        if (match is null)
            return NotFound();

        PlayerResponse[] players = ToPlayers(match);
        return Ok(
            new MatchDetailResponse(
                match.Id,
                match.Reservation.CourtId,
                match.Name ?? DefaultMatchName,
                match.SportId,
                match.Sport.Name,
                match.Reservation.Court.Name,
                match.Reservation.Court.Facility.Name,
                players
            )
        );
    }

    private static MatchListItemResponse ToMatchListItem(Match match) =>
        new(
            match.Id,
            match.Name ?? DefaultMatchName,
            match.SportId,
            match.Sport.Name,
            match.Reservation.Court.Name,
            match.Reservation.Court.Facility.Name,
            ToPlayers(match)
        );

    private static PlayerResponse[] ToPlayers(Match match) =>
        [.. match.MatchesUsers
            .Select(mu =>
                new PlayerResponse(mu.UserId, mu.User.Name, mu.IsFixedPosition, mu.TeamNumber)
            )];
}
