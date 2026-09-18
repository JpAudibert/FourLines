using FourLines.Application.Interfaces;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize(Roles = $"{RoleConstants.FacilityOwner}, {RoleConstants.Admin}")]
[Route("api/v{version:apiVersion}/owner/{ownerId}/facility/{facilityId}/[controller]")]
public class CourtsController(ILogger<CourtsController> logger, ICourtHandler courtHandler)
    : ApiControllerBase(logger)
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Court>>> GetAllCourtsFromFacility(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        CancellationToken cancellationToken
    )
    {
        const string operation = $"{nameof(CourtsController)}.{nameof(GetAllCourtsFromFacility)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["ownerId"] = ownerId,
                ["facilityId"] = facilityId,
            }
        );

        StartStopwatch();

        Result<IEnumerable<Court>> result = await courtHandler.GetAllCourtsFromFacility(
            facilityId,
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpGet("{courtId}")]
    public async Task<ActionResult<Court>> GetById(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromRoute] Guid courtId,
        CancellationToken cancellationToken
    )
    {
        const string operation = $"{nameof(CourtsController)}.{nameof(GetById)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["ownerId"] = ownerId,
                ["facilityId"] = facilityId,
                ["courtId"] = courtId,
            }
        );

        StartStopwatch();

        Result<Court> result = await courtHandler.GetCourtFromFacility(
            facilityId,
            courtId,
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<Court>> Create(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromBody] CreateCourtViewModel newCourt,
        CancellationToken cancellationToken
    )
    {
        const string operation = $"{nameof(CourtsController)}.{nameof(Create)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["ownerId"] = ownerId,
                ["facilityId"] = facilityId,
            }
        );

        StartStopwatch();

        Result<Court> result = await courtHandler.Create(
            new CreateCourtDTO()
            {
                OwnerId = ownerId,
                FacilityId = facilityId,
                SportId = newCourt.SportId,
                Name = newCourt.Name,
                IsActive = newCourt.IsActive,
            },
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpPut("{courtId}")]
    public async Task<ActionResult<Court>> Update(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromRoute] Guid courtId,
        [FromBody] UpdateCourtViewModel updateCourt,
        CancellationToken cancellationToken
    )
    {
        const string operation = $"{nameof(CourtsController)}.{nameof(Update)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["ownerId"] = ownerId,
                ["facilityId"] = facilityId,
                ["courtId"] = courtId,
            }
        );

        StartStopwatch();

        Result<Court> result = await courtHandler.Update(
            new UpdateCourtDTO()
            {
                Id = courtId,
                OwnerId = ownerId,
                FacilityId = facilityId,
                SportId = updateCourt.SportId,
                Name = updateCourt.Name,
                IsActive = updateCourt.IsActive,
            },
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpDelete("{courtId}")]
    public async Task<ActionResult<bool>> Delete(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromRoute] Guid courtId,
        CancellationToken cancellationToken
    )
    {
        const string operation = $"{nameof(CourtsController)}.{nameof(Delete)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["ownerId"] = ownerId,
                ["facilityId"] = facilityId,
                ["courtId"] = courtId,
            }
        );

        StartStopwatch();

        Result<bool> result = await courtHandler.Delete(
            new DeleteCourtDTO { FacilityId = facilityId, CourtId = courtId },
            cancellationToken
        );

        return HandleResult(result);
    }
}
