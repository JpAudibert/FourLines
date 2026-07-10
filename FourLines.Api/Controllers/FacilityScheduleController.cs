namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize(Roles = $"{RoleConstants.FacilityOwner}, {RoleConstants.Admin}")]
[Route("api/v{version:apiVersion}/owner/{ownerId}/facility/{facilityId}/[controller]")]
public class FacilityScheduleController(FacilityScheduleHandler facilityScheduleHandler) : ControllerBase
{
    private readonly FacilityScheduleHandler _facilityScheduleHandler = facilityScheduleHandler;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FacilitySchedule>>> GetScheduleFromFacility(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId)
    {
        Result<IEnumerable<FacilitySchedule>> result = await _facilityScheduleHandler.GetSchedules(ownerId, facilityId);

        if (result.IsFailure)
            return Problem(
                title: result.Error.Code,
                detail: result.Error.Description,
                statusCode: StatusCodes.Status400BadRequest);

        return Ok(result.Value);
    }

    [HttpPost]
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
            return Problem(
                title: result.Error.Code,
                detail: result.Error.Description,
                statusCode: StatusCodes.Status400BadRequest);

        return Ok(result.Value);
    }

    [HttpPost("multiple")]
    public async Task<ActionResult<Court>> CreateMultiple(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromBody] CreateFacilityScheduleViewModel[] newFacilitySchedules)
    {
        List<CreateFacilityScheduleDTO> schedules = [];

        foreach (var schedule in newFacilitySchedules)
        {
            CreateFacilityScheduleDTO facilityScheduleDTO = new()
            {
                OwnerId = ownerId,
                FacilityId = facilityId,
                DayOfWeek = schedule.DayOfWeek,
                OpensAt = schedule.OpensAt,
                ClosesAt = schedule.ClosesAt,
            };

            schedules.Add(facilityScheduleDTO);
        }

        Result<IEnumerable<FacilitySchedule>> result = await _facilityScheduleHandler.CreateMultiple(schedules);

        if (result.IsFailure)
            return Problem(
                title: result.Error.Code,
                detail: result.Error.Description,
                statusCode: StatusCodes.Status400BadRequest);

        return Ok(result.Value);
    }

    [HttpPut("{scheduleId}")]
    public async Task<ActionResult<FacilitySchedule>> Update(
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
            return Problem(
                title: result.Error.Code,
                detail: result.Error.Description,
                statusCode: StatusCodes.Status400BadRequest);

        return Ok(result.Value);
    }

    [HttpDelete("{scheduleId}")]
    public async Task<ActionResult<bool>> Delete(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromRoute] Guid scheduleId)
    {
        Result<bool> result = await _facilityScheduleHandler.Delete(ownerId, facilityId, scheduleId);

        if (result.IsFailure)
            return Problem(
                title: result.Error.Code,
                detail: result.Error.Description,
                statusCode: StatusCodes.Status400BadRequest);

        return Ok(result.Value);
    }
}
