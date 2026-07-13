namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize(Roles = $"{RoleConstants.FacilityOwner}, {RoleConstants.Admin}")]
[Route("api/v{version:apiVersion}/owner/{ownerId}/facility/{facilityId}/[controller]")]
public class CourtController(ILogger<CourtController> logger, CourtHandler courtHandler) : ApiControllerBase
{
    private readonly ILogger<CourtController> _logger = logger;
    private readonly CourtHandler _courtHandler = courtHandler;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Court>>> GetAllCourtsFromFacility(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId)
    {
        const string operation = $"{nameof(CourtController)}.{nameof(GetAllCourtsFromFacility)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["ownerId"] = ownerId,
            ["facilityId"] = facilityId,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<IEnumerable<Court>> result = await _courtHandler.GetAllCourtsFromFacility(ownerId, facilityId);

        return HandleResult(result, _logger, operation, sw);
    }

    [HttpGet("{courtId}")]
    public async Task<ActionResult<Court>> GetById(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromRoute] Guid courtId)
    {
        const string operation = $"{nameof(CourtController)}.{nameof(GetById)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["ownerId"] = ownerId,
            ["facilityId"] = facilityId,
            ["courtId"] = courtId,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<Court> result = await _courtHandler.GetFacility(ownerId, facilityId, courtId);

        return HandleResult(result, _logger, operation, sw);
    }

    [HttpPost]
    public async Task<ActionResult<Court>> Create(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromBody] CreateCourtViewModel newCourt)
    {
        const string operation = $"{nameof(CourtController)}.{nameof(Create)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["ownerId"] = ownerId,
            ["facilityId"] = facilityId,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<Court> result = await _courtHandler.Create(new CreateCourtDTO()
        {
            OwnerId = ownerId,
            FacilityId = facilityId,
            SportId = newCourt.SportId,
            Name = newCourt.Name,
            IsActive = newCourt.IsActive,
        });

        return HandleResult(result, _logger, operation, sw);
    }

    [HttpPut("{courtId}")]
    public async Task<ActionResult<Court>> Update(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromRoute] Guid courtId,
        [FromBody] UpdateCourtViewModel updateCourt)
    {
        const string operation = $"{nameof(CourtController)}.{nameof(Update)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["ownerId"] = ownerId,
            ["facilityId"] = facilityId,
            ["courtId"] = courtId,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<Court> result = await _courtHandler.Update(new UpdateCourtDTO()
        {
            Id = courtId,
            OwnerId = ownerId,
            FacilityId = facilityId,
            SportId = updateCourt.SportId,
            Name = updateCourt.Name,
            IsActive = updateCourt.IsActive,
        });

        return HandleResult(result, _logger, operation, sw);
    }

    [HttpDelete("{courtId}")]
    public async Task<ActionResult<bool>> Delete(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromRoute] Guid courtId)
    {
        const string operation = $"{nameof(CourtController)}.{nameof(Delete)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["ownerId"] = ownerId,
            ["facilityId"] = facilityId,
            ["courtId"] = courtId,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<bool> result = await _courtHandler.Delete(ownerId, facilityId, courtId);

        return HandleResult(result, _logger, operation, sw);
    }
}
