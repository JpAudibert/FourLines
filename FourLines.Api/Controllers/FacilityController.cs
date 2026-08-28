using FourLines.Application.Interfaces;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize(Roles = $"{RoleConstants.FacilityOwner}, {RoleConstants.Admin}")]
[Route("api/v{version:apiVersion}/owner/{ownerId}/[controller]")]
public class FacilityController(ILogger<FacilityController> logger, FacilityHandler facilityHandler)
    : ApiControllerBase(logger)
{
    private readonly ILogger<FacilityController> _logger = logger;
    private readonly IFacilityHandler _facilityHandler = facilityHandler;

    [HttpGet("~/api/v{version:apiVersion}/facilities")]
    [EndpointName("GetAll")]
    public async Task<ActionResult<IEnumerable<Facility>>> GetAllFromFacilities()
    {
        const string operation = $"{nameof(FacilityController)}.{nameof(GetAllFromFacilities)}";
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            ["operation"] = operation,
        });

        StartStopwatch();

        Result<IEnumerable<Facility>> result = await _facilityHandler.GetAllFacilities();

        return HandleResult(result);
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

        StartStopwatch();

        Result<IEnumerable<Facility>> result = await _facilityHandler.GetFacilitiesFromOwner(ownerId);

        return HandleResult(result);
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

        StartStopwatch();

        Result<Facility> result = await _facilityHandler.GetFacilityFromOwner(ownerId, facilityId);

        return HandleResult(result);
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

        StartStopwatch();

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

        return HandleResult(result);
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

        StartStopwatch();

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

        return HandleResult(result);
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

        StartStopwatch();

        Result<bool> result = await _facilityHandler.Delete(new DeleteFacilityDTO()
        {
            OwnerId = ownerId,
            FacilityId = facilityId
        });

        return HandleResult(result);
    }
}
