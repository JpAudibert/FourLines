using FourLines.Application.Interfaces;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize(Roles = $"{RoleConstants.FacilityOwner}, {RoleConstants.Admin}")]
[Route("api/v{version:apiVersion}/owner/{ownerId}/facility/{facilityId}/[controller]")]
public class FacilityScheduleController(ILogger<FacilityScheduleController> logger, IFacilityScheduleHandler facilityScheduleHandler)
    : ApiControllerBase(logger)
{
    private readonly ILogger<FacilityScheduleController> _logger = logger;
    private readonly IFacilityScheduleHandler _facilityScheduleHandler = facilityScheduleHandler;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FacilitySchedule>>> GetScheduleFromFacility(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId)
    {
        const string operation = $"{nameof(FacilityScheduleController)}.{nameof(GetScheduleFromFacility)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["ownerId"] = ownerId,
            ["facilityId"] = facilityId,
        });

        StartStopwatch();

        Result<IEnumerable<FacilitySchedule>> result = await _facilityScheduleHandler.GetSchedules(ownerId, facilityId);

        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<FacilitySchedule>> Create(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromBody] CreateFacilityScheduleViewModel newFacilitySchedule)
    {
        const string operation = $"{nameof(FacilityScheduleController)}.{nameof(Create)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["ownerId"] = ownerId,
            ["facilityId"] = facilityId,
        });

        StartStopwatch();

        Result<FacilitySchedule> result = await _facilityScheduleHandler.Create(new CreateFacilityScheduleDTO()
        {
            OwnerId = ownerId,
            FacilityId = facilityId,
            DayOfWeek = newFacilitySchedule.DayOfWeek,
            OpensAt = newFacilitySchedule.OpensAt,
            ClosesAt = newFacilitySchedule.ClosesAt,
        });

        return HandleResult(result);
    }

    [HttpPost("multiple")]
    public async Task<ActionResult<IEnumerable<FacilitySchedule>>> CreateMultiple(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromBody] CreateFacilityScheduleViewModel[] newFacilitySchedules)
    {
        const string operation = $"{nameof(FacilityScheduleController)}.{nameof(CreateMultiple)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["ownerId"] = ownerId,
            ["facilityId"] = facilityId,
        });

        StartStopwatch();

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

        return HandleResult(result);
    }

    [HttpPut("{scheduleId}")]
    public async Task<ActionResult<FacilitySchedule>> Update(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromRoute] Guid scheduleId,
        [FromBody] UpdateFacilityScheduleViewModel updateFacilitySchedule)
    {
        const string operation = $"{nameof(FacilityScheduleController)}.{nameof(Update)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["ownerId"] = ownerId,
            ["facilityId"] = facilityId,
            ["scheduleId"] = scheduleId,
        });

        StartStopwatch();

        Result<FacilitySchedule> result = await _facilityScheduleHandler.Update(new UpdateFacilityScheduleDTO()
        {
            Id = scheduleId,
            OwnerId = ownerId,
            FacilityId = facilityId,
            DayOfWeek = updateFacilitySchedule.DayOfWeek,
            OpensAt = updateFacilitySchedule.OpensAt,
            ClosesAt = updateFacilitySchedule.ClosesAt,
        });

        return HandleResult(result);
    }

    [HttpDelete("{scheduleId}")]
    public async Task<ActionResult<bool>> Delete(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromRoute] Guid scheduleId)
    {
        const string operation = $"{nameof(FacilityScheduleController)}.{nameof(Delete)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["ownerId"] = ownerId,
            ["facilityId"] = facilityId,
            ["scheduleId"] = scheduleId,
        });

        StartStopwatch();

        Result<bool> result = await _facilityScheduleHandler.Delete(new DeleteFacilityScheduleDTO
        {
            OwnerId = ownerId,
            FacilityId = facilityId,
            ScheduleId = scheduleId
        });

        return HandleResult(result);
    }
}
