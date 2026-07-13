namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/user/{userId}/[controller]")]
public class ReservationController(ILogger<ReservationController> logger, ReservationHandler reservationHandler)
    : ApiControllerBase
{
    private readonly ILogger<ReservationController> _logger = logger;
    private readonly ReservationHandler _reservationHandler = reservationHandler;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservationsFromUser(
        [FromRoute] Guid userId)
    {
        const string operation = $"{nameof(ReservationController)}.{nameof(GetAllReservationsFromUser)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["userId"] = userId,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<IEnumerable<Reservation>> result = await _reservationHandler.GetAllReservationsFromUser(userId);

        return HandleResult(result, _logger, operation, sw);
    }

    [HttpGet("~/api/v{version:apiVersion}/court/{courtId}/[controller]")]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservationsFromCourt(
        [FromRoute] Guid courtId)
    {
        const string operation = $"{nameof(ReservationController)}.{nameof(GetAllReservationsFromCourt)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["courtId"] = courtId,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<IEnumerable<Reservation>> result = await _reservationHandler.GetAllReservationsFromCourt(courtId);

        return HandleResult(result, _logger, operation, sw);
    }

    [HttpGet("{reservationId}")]
    public async Task<ActionResult<Reservation>> GetOneReservationsFromUser(
        [FromRoute] Guid userId,
        [FromRoute] Guid reservationId)
    {
        const string operation = $"{nameof(ReservationController)}.{nameof(GetOneReservationsFromUser)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["userId"] = userId,
            ["reservationId"] = reservationId,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<Reservation> result = await _reservationHandler.GetOneReservationFromUser(userId, reservationId);

        return HandleResult(result, _logger, operation, sw);
    }

    [HttpPost]
    public async Task<ActionResult<Reservation>> CreateAReservationForUser(
        [FromRoute] Guid userId,
        [FromBody] CreateReservationViewModel newReservation)
    {
        const string operation = $"{nameof(ReservationController)}.{nameof(CreateAReservationForUser)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["userId"] = userId,
            ["courtId"] = newReservation.CourtId,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<Reservation> result = await _reservationHandler.Create(new CreateReservationDTO()
        {
            UserId = userId,
            CourtId = newReservation.CourtId,
            Period = newReservation.Period,
            Status = newReservation.Status,
        });

        return HandleResult(result, _logger, operation, sw);
    }

    [HttpPatch("{reservationId}")]
    public async Task<ActionResult<Reservation>> UpdateStatusFromReservation(
        [FromRoute] Guid userId,
        [FromRoute] Guid reservationId,
        [FromBody] UpdateReservationStatusViewModel updateReservation)
    {
        const string operation = $"{nameof(ReservationController)}.{nameof(UpdateStatusFromReservation)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["userId"] = userId,
            ["reservationId"] = reservationId,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<Reservation> result = await _reservationHandler.UpdateStatusFromReservation(new UpdateStatusFromReservationDTO()
        {
            Id = reservationId,
            UserId = userId,
            Status = updateReservation.Status,
        });

        return HandleResult(result, _logger, operation, sw);
    }

    [HttpDelete("{reservationId}")]
    public async Task<ActionResult<bool>> DeleteAReservationFromUser(
        [FromRoute] Guid userId,
        [FromRoute] Guid reservationId)
    {
        const string operation = $"{nameof(ReservationController)}.{nameof(DeleteAReservationFromUser)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["userId"] = userId,
            ["reservationId"] = reservationId,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<bool> result = await _reservationHandler.Delete(userId, reservationId);

        return HandleResult(result, _logger, operation, sw);
    }
}
