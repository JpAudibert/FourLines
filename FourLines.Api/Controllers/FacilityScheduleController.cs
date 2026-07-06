using FourLines.Api.ViewModels.FacilitySchedules;
using FourLines.Application.DTOs.FacilitySchedules;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}")]
public class FacilityScheduleController(FacilityScheduleHandler facilityScheduleHandler) : Controller
{
    private readonly FacilityScheduleHandler _facilityScheduleHandler = facilityScheduleHandler;

    [HttpGet("owner/{ownerId}/facility/{facilityId}")]
    public async Task<IActionResult> GetScheduleFromFacility(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId)
    {
        Result<IEnumerable<FacilitySchedule>> result = await _facilityScheduleHandler.GetSchedules(ownerId, facilityId);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost("owner/{ownerId}/facility/{facilityId}")]
    public async Task<ActionResult<Court>> Create(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromBody] CreateFacilityScheduleViewModel newFacilitySchedule)
    {
        Result<FacilitySchedule> result = await _facilityScheduleHandler.Create(new CreateFacilityScheduleDTO()
        {
            OwnerId = ownerId,
            FacilityId = facilityId,
            DayOfWeek = newFacilitySchedule.DayOfWeek,
            OpensAt = newFacilitySchedule.OpensAt,
            ClosesAt = newFacilitySchedule.ClosesAt,
        });

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    //[HttpPost("owner/{ownerId}/facility/{facilityId}/multiple")]
    //public async Task<ActionResult<Court>> CreateMultiple(
    //    [FromRoute] Guid ownerId,
    //    [FromRoute] Guid facilityId,
    //    [FromBody] CreateFacilityScheduleViewModel[] newFacilitySchedules)
    //{
    //    Result<FacilitySchedule> result = await _facilityScheduleHandler.Create(new CreateFacilityScheduleDTO()
    //    {
    //        OwnerId = ownerId,
    //        FacilityId = facilityId,
    //        DayOfWeek = newFacilitySchedule.DayOfWeek,
    //        OpensAt = newFacilitySchedule.OpensAt,
    //        ClosesAt = newFacilitySchedule.ClosesAt,
    //    });

    //    if (result.IsFailure)
    //        return BadRequest(result.Error);

    //    return Ok(result.Value);
    //}

    [HttpPut("owner/{ownerId}/facility/{facilityId}/court/{scheduleId}")]
    public async Task<ActionResult<Court>> Update(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromRoute] Guid scheduleId,
        [FromBody] UpdateFacilityScheduleViewModel updateFacilitySchedule)
    {
        Result<FacilitySchedule> result = await _facilityScheduleHandler.Update(new UpdateFacilityScheduleDTO()
        {
            Id = scheduleId,
            OwnerId = ownerId,
            FacilityId = facilityId,
            DayOfWeek = updateFacilitySchedule.DayOfWeek,
            OpensAt = updateFacilitySchedule.OpensAt,
            ClosesAt = updateFacilitySchedule.ClosesAt,
        });

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpDelete("owner/{ownerId}/facility/{facilityId}/court/{scheduleId}")]
    public async Task<ActionResult<bool>> Delete(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromRoute] Guid scheduleId)
    {
        Result<bool> result = await _facilityScheduleHandler.Delete(ownerId, facilityId, scheduleId);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
