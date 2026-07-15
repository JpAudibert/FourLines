namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize(Roles = $"{RoleConstants.FacilityOwner}, {RoleConstants.Admin}")]
[Route("api/v{version:apiVersion}/owner/{ownerId}/[controller]")]
public class FacilityController(ILogger<FacilityController> logger, FacilityHandler facilityHandler) : ApiControllerBase
{
    private readonly ILogger<FacilityController> _logger = logger;
    private readonly FacilityHandler _facilityHandler = facilityHandler;

    [HttpGet("~/api/v{version:apiVersion}/facilities")]
    [EndpointName("GetAll")]
    public async Task<ActionResult<IEnumerable<Facility>>> GetAllFromFacilities()
    {
        const string operation = $"{nameof(FacilityController)}.{nameof(GetAllFromFacilities)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<IEnumerable<Facility>> result = await _facilityHandler.GetAllFacilities();

        return HandleResult(result, _logger, operation, sw);
    }

    [HttpGet]
    [EndpointName("GetAllFromOwner")]
    public async Task<ActionResult<IEnumerable<Facility>>> GetAllFromOwner([FromRoute] Guid ownerId)
    {
        const string operation = $"{nameof(FacilityController)}.{nameof(GetAllFromOwner)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["ownerId"] = ownerId,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<IEnumerable<Facility>> result = await _facilityHandler.GetFacilitiesFromOwner(ownerId);

        return HandleResult(result, _logger, operation, sw);
    }

    [HttpGet("{facilityId}")]
    [EndpointName("GetFacilityFromOwner")]
    public async Task<ActionResult<Facility>> GetFacilityFromOwner([FromRoute] Guid ownerId, [FromRoute] Guid facilityId)
    {
        const string operation = $"{nameof(FacilityController)}.{nameof(GetFacilityFromOwner)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["ownerId"] = ownerId,
            ["facilityId"] = facilityId,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<Facility> result = await _facilityHandler.GetFacilityFromOwner(ownerId, facilityId);

        return HandleResult(result, _logger, operation, sw);
    }

    [HttpPost]
    [EndpointName("Create")]
    public async Task<ActionResult<Facility>> Create([FromRoute] Guid ownerId, [FromBody] CreateFacilityViewModel request)
    {
        const string operation = $"{nameof(FacilityController)}.{nameof(Create)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["ownerId"] = ownerId,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<Facility> result = await _facilityHandler.Create(new CreateFacilityDTO()
        {
            OwnerId = ownerId,
            Name = request.Name,
            Address = request.Address,
            City = request.City,
            State = request.State,
            ZipCode = request.ZipCode,
            RegistrationNumber = request.RegistrationNumber,
        });

        return HandleResult(result, _logger, operation, sw);
    }

    [HttpPut("{facilityId}")]
    [EndpointName("Update")]
    public async Task<ActionResult<Facility>> Update(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromBody] UpdateFacilityViewModel facility)
    {
        const string operation = $"{nameof(FacilityController)}.{nameof(Update)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["ownerId"] = ownerId,
            ["facilityId"] = facilityId,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<Facility> result = await _facilityHandler.Update(new UpdateFacilityDTO()
        {
            Id = facilityId,
            OwnerId = ownerId,
            Name = facility.Name,
            Address = facility.Address,
            City = facility.City,
            State = facility.State,
            ZipCode = facility.ZipCode,
            RegistrationNumber = facility.RegistrationNumber
        });

        return HandleResult(result, _logger, operation, sw);
    }

    [HttpDelete("{facilityId}")]
    [EndpointName("Delete")]
    public async Task<ActionResult<bool>> Delete([FromRoute] Guid ownerId, [FromRoute] Guid facilityId)
    {
        const string operation = $"{nameof(FacilityController)}.{nameof(Delete)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
            ["ownerId"] = ownerId,
            ["facilityId"] = facilityId,
        });

        Stopwatch sw = Stopwatch.StartNew();

        Result<bool> result = await _facilityHandler.Delete(ownerId, facilityId);

        return HandleResult(result, _logger, operation, sw);
    }
}
