using FourLines.Api.ViewModels.Reservations;
using FourLines.Application.DTOs.Reservations;

namespace FourLines.Api.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/user/{userId}/[controller]")]
    public class ReservationController(ReservationHandler reservationHandler) : ControllerBase
    {
        private readonly ReservationHandler _reservationHandler = reservationHandler;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservationsFromUser(
            [FromRoute] Guid userId)
        {
            Result<IEnumerable<Reservation>> result = await _reservationHandler.GetAllReservationsFromUser(userId);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("~/api/v{version:apiVersion}/court/{courtId}/[controller]")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservationsFromCourt(
            [FromRoute] Guid courtId)
        {
            Result<IEnumerable<Reservation>> result = await _reservationHandler.GetAllReservationsFromCourt(courtId);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpGet("{reservationId}")]
        public async Task<ActionResult<Reservation>> GetOneReservationsFromUser(
            [FromRoute] Guid userId,
            [FromRoute] Guid reservationId)
        {
            Result<Reservation> result = await _reservationHandler.GetOneReservationFromUser(userId, reservationId);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<ActionResult<Reservation>> CreateAReservationForUser(
            [FromRoute] Guid userId,
            [FromBody] CreateReservationViewModel newReservation)
        {
            Result<Reservation> result = await _reservationHandler.Create(new CreateReservationDTO()
            {
                UserId = userId,
                CourtId = newReservation.CourtId,
                Period = newReservation.Period,
                Status = newReservation.Status,
            });

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        [HttpDelete("{reservationId}")]
        public async Task<ActionResult<bool>> DeleteAReservationFromUser(
            [FromRoute] Guid userId,
            [FromRoute] Guid reservationId)
        {
            Result<bool> result = await _reservationHandler.Delete(userId, reservationId);

            if (result.IsFailure)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }
    }
}
