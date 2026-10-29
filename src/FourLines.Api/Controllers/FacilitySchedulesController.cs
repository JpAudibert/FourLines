using FourLines.Application.Interfaces;
using FourLines.Domain.Results.ErrorResults;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize(Roles = $"{RoleConstants.FacilityOwner}, {RoleConstants.Admin}")]
[Route("api/v{version:apiVersion}/facilities/{facilityId:guid}/schedules")]
public class FacilitySchedulesController(
    ILogger<FacilitySchedulesController> logger,
    IFacilityScheduleHandler facilityScheduleHandler,
    ICurrentUserProvider currentUserProvider
) : ApiControllerBase(logger, currentUserProvider)
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FacilitySchedule>>> GetScheduleFromFacility(
        [FromRoute] Guid facilityId,
        CancellationToken cancellationToken = default
    )
    {
        if (!TryGetCurrentUser(out Guid ownerId, out bool isAdmin))
            return Unauthorized();

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
            ownerId,
            isAdmin,
            facilityId,
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<FacilitySchedule>> Create(
        [FromRoute] Guid facilityId,
        [FromBody] CreateFacilityScheduleRequest newFacilitySchedule,
        CancellationToken cancellationToken = default
    )
    {
        if (!TryGetCurrentUser(out Guid ownerId, out bool isAdmin))
            return Unauthorized();

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
                OwnerId = ownerId,
                IsAdmin = isAdmin,
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
        [FromRoute] Guid facilityId,
        [FromBody] CreateFacilityScheduleRequest[] newFacilitySchedules,
        CancellationToken cancellationToken = default
    )
    {
        if (!TryGetCurrentUser(out Guid ownerId, out bool isAdmin))
            return Unauthorized();

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

        if (newFacilitySchedules.Length == 0)
        {
            return HandleResult(
                Result<IEnumerable<FacilitySchedule>>.Failure(
                    FacilitySchedulesErrorResults.NoSchedulesProvided
                )
            );
        }

        List<CreateFacilityScheduleDTO> schedules = [];

        foreach (var schedule in newFacilitySchedules)
        {
            CreateFacilityScheduleDTO facilityScheduleDTO = new()
            {
                OwnerId = ownerId,
                IsAdmin = isAdmin,
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
        [FromRoute] Guid facilityId,
        [FromRoute] Guid scheduleId,
        [FromBody] UpdateFacilityScheduleRequest updateFacilitySchedule,
        CancellationToken cancellationToken = default
    )
    {
        if (!TryGetCurrentUser(out Guid ownerId, out bool isAdmin))
            return Unauthorized();

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
                OwnerId = ownerId,
                IsAdmin = isAdmin,
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
        [FromRoute] Guid facilityId,
        [FromRoute] Guid scheduleId,
        CancellationToken cancellationToken = default
    )
    {
        if (!TryGetCurrentUser(out Guid ownerId, out bool isAdmin))
            return Unauthorized();

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
            new DeleteFacilityScheduleDTO
            {
                OwnerId = ownerId,
                IsAdmin = isAdmin,
                FacilityId = facilityId,
                ScheduleId = scheduleId,
            },
            cancellationToken
        );

        return HandleResult(result);
    }
}
