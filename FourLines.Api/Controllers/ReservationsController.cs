using FourLines.Application.Interfaces;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/user/{userId}/[controller]")]
public class ReservationsController(
    ILogger<ReservationsController> logger,
    IReservationHandler reservationHandler
) : ApiControllerBase(logger)
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservationsFromUser(
        [FromRoute] Guid userId,
        CancellationToken cancellationToken
    )
    {
        const string operation =
            $"{nameof(ReservationsController)}.{nameof(GetAllReservationsFromUser)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object> { ["operation"] = operation, ["userId"] = userId }
        );

        StartStopwatch();

        Result<IEnumerable<Reservation>> result =
            await reservationHandler.GetAllReservationsFromUser(userId, cancellationToken);

        return HandleResult(result);
    }

    [HttpGet("~/api/v{version:apiVersion}/court/{courtId}/[controller]")]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservationsFromCourt(
        [FromRoute] Guid courtId,
        CancellationToken cancellationToken
    )
    {
        const string operation =
            $"{nameof(ReservationsController)}.{nameof(GetAllReservationsFromCourt)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object> { ["operation"] = operation, ["courtId"] = courtId }
        );

        StartStopwatch();

        Result<IEnumerable<Reservation>> result =
            await reservationHandler.GetAllReservationsFromCourt(courtId, cancellationToken);

        return HandleResult(result);
    }

    [HttpGet("{reservationId}")]
    public async Task<ActionResult<Reservation>> GetOneReservationsFromUser(
        [FromRoute] Guid userId,
        [FromRoute] Guid reservationId,
        CancellationToken cancellationToken
    )
    {
        const string operation =
            $"{nameof(ReservationsController)}.{nameof(GetOneReservationsFromUser)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["userId"] = userId,
                ["reservationId"] = reservationId,
            }
        );

        StartStopwatch();

        Result<Reservation> result = await reservationHandler.GetOneReservationFromUser(
            userId,
            reservationId,
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<ConfirmReservationResponseDTO>> CreateAReservationForUser(
        [FromRoute] Guid userId,
        [FromBody] CreateReservationViewModel newReservation,
        CancellationToken cancellationToken
    )
    {
        const string operation =
            $"{nameof(ReservationsController)}.{nameof(CreateAReservationForUser)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["userId"] = userId,
                ["courtId"] = newReservation.CourtId,
            }
        );

        StartStopwatch();

        Result<ConfirmReservationResponseDTO> result = await reservationHandler.Create(
            new CreateReservationDTO()
            {
                UserId = userId,
                CourtId = newReservation.CourtId,
                Period = newReservation.Period,
                Status = newReservation.Status,
            },
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpPatch("{reservationId}")]
    public async Task<ActionResult<Reservation>> UpdateStatusFromReservation(
        [FromRoute] Guid userId,
        [FromRoute] Guid reservationId,
        [FromBody] UpdateReservationStatusViewModel updateReservation,
        CancellationToken cancellationToken
    )
    {
        const string operation =
            $"{nameof(ReservationsController)}.{nameof(UpdateStatusFromReservation)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["userId"] = userId,
                ["reservationId"] = reservationId,
            }
        );

        StartStopwatch();

        Result<Reservation> result = await reservationHandler.UpdateReservationStatus(
            new UpdateStatusFromReservationDTO()
            {
                Id = reservationId,
                UserId = userId,
                Status = updateReservation.Status,
            },
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpDelete("{reservationId}")]
    public async Task<ActionResult<bool>> DeleteAReservationFromUser(
        [FromRoute] Guid userId,
        [FromRoute] Guid reservationId,
        CancellationToken cancellationToken
    )
    {
        const string operation =
            $"{nameof(ReservationsController)}.{nameof(DeleteAReservationFromUser)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["userId"] = userId,
                ["reservationId"] = reservationId,
            }
        );

        StartStopwatch();

        Result<bool> result = await reservationHandler.Delete(
            new DeleteReservationDTO { UserId = userId, ReservationId = reservationId },
            cancellationToken
        );

        return HandleResult(result);
    }
}
