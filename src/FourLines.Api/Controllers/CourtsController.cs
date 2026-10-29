using FourLines.Application.Interfaces;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize(Roles = $"{RoleConstants.FacilityOwner}, {RoleConstants.Admin}")]
[Route("api/v{version:apiVersion}/facilities/{facilityId:guid}/courts")]
public class CourtsController(
    ILogger<CourtsController> logger,
    ICourtHandler courtHandler,
    ICurrentUserProvider currentUserProvider
) : ApiControllerBase(logger, currentUserProvider)
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Court>>> GetAllCourtsFromFacility(
        [FromRoute] Guid facilityId,
        CancellationToken cancellationToken
    )
    {
        if (!TryGetCurrentUser(out Guid ownerId, out bool isAdmin))
            return Unauthorized();

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
            ownerId,
            isAdmin,
            facilityId,
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpGet("{courtId}")]
    public async Task<ActionResult<Court>> GetById(
        [FromRoute] Guid facilityId,
        [FromRoute] Guid courtId,
        CancellationToken cancellationToken
    )
    {
        if (!TryGetCurrentUser(out Guid ownerId, out bool isAdmin))
            return Unauthorized();

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
            ownerId,
            isAdmin,
            facilityId,
            courtId,
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<Court>> Create(
        [FromRoute] Guid facilityId,
        [FromBody] CreateCourtRequest newCourt,
        CancellationToken cancellationToken
    )
    {
        if (!TryGetCurrentUser(out Guid ownerId, out bool isAdmin))
            return Unauthorized();

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
                IsAdmin = isAdmin,
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
        [FromRoute] Guid facilityId,
        [FromRoute] Guid courtId,
        [FromBody] UpdateCourtRequest updateCourt,
        CancellationToken cancellationToken
    )
    {
        if (!TryGetCurrentUser(out Guid ownerId, out bool isAdmin))
            return Unauthorized();

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
                IsAdmin = isAdmin,
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
        [FromRoute] Guid facilityId,
        [FromRoute] Guid courtId,
        CancellationToken cancellationToken
    )
    {
        if (!TryGetCurrentUser(out Guid ownerId, out bool isAdmin))
            return Unauthorized();

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
            new DeleteCourtDTO
            {
                OwnerId = ownerId,
                IsAdmin = isAdmin,
                FacilityId = facilityId,
                CourtId = courtId,
            },
            cancellationToken
        );

        return HandleResult(result);
    }
}
