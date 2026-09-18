using FourLines.Application.DTOs.FacilitySchedules.Interfaces;
using FourLines.Application.Interfaces;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize(Roles = $"{RoleConstants.FacilityOwner}, {RoleConstants.Admin}")]
[Route("api/v{version:apiVersion}/owner/{ownerId}/facility/{facilityId}/[controller]")]
public class FacilitySchedulesController(
    ILogger<FacilitySchedulesController> logger,
    IFacilityScheduleHandler facilityScheduleHandler
) : ApiControllerBase(logger)
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FacilitySchedule>>> GetScheduleFromFacility(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        CancellationToken cancellationToken = default
    )
    {
        const string operation =
            $"{nameof(FacilitySchedulesController)}.{nameof(GetScheduleFromFacility)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["ownerId"] = ownerId,
                ["facilityId"] = facilityId,
            }
        );

        StartStopwatch();

        Result<IEnumerable<FacilitySchedule>> result = await facilityScheduleHandler.GetSchedules(
            facilityId,
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<FacilitySchedule>> Create(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromBody] CreateFacilityScheduleViewModel newFacilitySchedule,
        CancellationToken cancellationToken = default
    )
    {
        const string operation = $"{nameof(FacilitySchedulesController)}.{nameof(Create)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["ownerId"] = ownerId,
                ["facilityId"] = facilityId,
            }
        );

        StartStopwatch();

        Result<FacilitySchedule> result = await facilityScheduleHandler.Create(
            new CreateFacilityScheduleDTO()
            {
                FacilityId = facilityId,
                DayOfWeek = newFacilitySchedule.DayOfWeek,
                OpensAt = newFacilitySchedule.OpensAt,
                ClosesAt = newFacilitySchedule.ClosesAt,
            },
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpPost("multiple")]
    public async Task<ActionResult<IEnumerable<FacilitySchedule>>> CreateMultiple(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromBody] CreateFacilityScheduleViewModel[] newFacilitySchedules,
        CancellationToken cancellationToken = default
    )
    {
        const string operation = $"{nameof(FacilitySchedulesController)}.{nameof(CreateMultiple)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["ownerId"] = ownerId,
                ["facilityId"] = facilityId,
            }
        );

        StartStopwatch();

        List<ICreateFacilityScheduleDTO> schedules = [];

        foreach (var schedule in newFacilitySchedules)
        {
            CreateFacilityScheduleDTO facilityScheduleDTO = new()
            {
                FacilityId = facilityId,
                DayOfWeek = schedule.DayOfWeek,
                OpensAt = schedule.OpensAt,
                ClosesAt = schedule.ClosesAt,
            };

            schedules.Add(facilityScheduleDTO);
        }

        Result<IEnumerable<FacilitySchedule>> result = await facilityScheduleHandler.CreateMultiple(
            schedules,
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpPut("{scheduleId}")]
    public async Task<ActionResult<FacilitySchedule>> Update(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromRoute] Guid scheduleId,
        [FromBody] UpdateFacilityScheduleViewModel updateFacilitySchedule,
        CancellationToken cancellationToken = default
    )
    {
        const string operation = $"{nameof(FacilitySchedulesController)}.{nameof(Update)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["ownerId"] = ownerId,
                ["facilityId"] = facilityId,
                ["scheduleId"] = scheduleId,
            }
        );

        StartStopwatch();

        Result<FacilitySchedule> result = await facilityScheduleHandler.Update(
            new UpdateFacilityScheduleDTO()
            {
                Id = scheduleId,
                FacilityId = facilityId,
                DayOfWeek = updateFacilitySchedule.DayOfWeek,
                OpensAt = updateFacilitySchedule.OpensAt,
                ClosesAt = updateFacilitySchedule.ClosesAt,
            },
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpDelete("{scheduleId}")]
    public async Task<ActionResult<bool>> Delete(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromRoute] Guid scheduleId,
        CancellationToken cancellationToken = default
    )
    {
        const string operation = $"{nameof(FacilitySchedulesController)}.{nameof(Delete)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["ownerId"] = ownerId,
                ["facilityId"] = facilityId,
                ["scheduleId"] = scheduleId,
            }
        );

        StartStopwatch();

        Result<bool> result = await facilityScheduleHandler.Delete(
            new DeleteFacilityScheduleDTO { FacilityId = facilityId, ScheduleId = scheduleId },
            cancellationToken
        );

        return HandleResult(result);
    }
}
