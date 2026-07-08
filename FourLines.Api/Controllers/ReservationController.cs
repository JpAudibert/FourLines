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
            // Logic to retrieve all reservations for the authenticated user
            return Ok();
        }

        [HttpGet("~/api/v{version:apiVersion}/court/{courtId}/[controller]")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservationsFromCourt(
            [FromRoute] Guid courtId)
        {
            // Logic to retrieve all reservations for the authenticated user
            return Ok();
        }

        [HttpGet("{reservationId}")]
        public async Task<ActionResult<Reservation>> GetOneReservationsFromUser(
            [FromRoute] Guid userId,
            [FromRoute] Guid reservationId)
        {
            // Logic to retrieve all reservations for the authenticated user
            return Ok();
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
            // Logic to retrieve all reservations for the authenticated user
            return Ok();
        }
    }
}
