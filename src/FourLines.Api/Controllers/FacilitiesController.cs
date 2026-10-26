using FourLines.Application.Interfaces;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize(Roles = $"{RoleConstants.FacilityOwner}, {RoleConstants.Admin}")]
[Route("api/v{version:apiVersion}/owner/{ownerId}/[controller]")]
public class FacilitiesController(
    ILogger<FacilitiesController> logger,
    IFacilityHandler facilityHandler
) : ApiControllerBase(logger)
{
    [HttpGet("~/api/v{version:apiVersion}/facilities")]
    [EndpointName("GetAll")]
    public async Task<ActionResult<IEnumerable<Facility>>> GetAllFromFacilities(
        CancellationToken cancellationToken = default
    )
    {
        const string operation = $"{nameof(FacilitiesController)}.{nameof(GetAllFromFacilities)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object> { ["operation"] = operation }
        );

        StartStopwatch();

        Result<IEnumerable<Facility>> result = await facilityHandler.GetAllFacilities(
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpGet]
    [EndpointName("GetAllFromOwner")]
    public async Task<ActionResult<IEnumerable<Facility>>> GetAllFromOwner(
        [FromRoute] Guid ownerId,
        CancellationToken cancellationToken = default
    )
    {
        const string operation = $"{nameof(FacilitiesController)}.{nameof(GetAllFromOwner)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object> { ["operation"] = operation, ["ownerId"] = ownerId }
        );

        StartStopwatch();

        Result<IEnumerable<Facility>> result = await facilityHandler.GetFacilitiesFromOwner(
            ownerId,
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpGet("{facilityId}")]
    [EndpointName("GetFacilityFromOwner")]
    public async Task<ActionResult<Facility>> GetFacilityFromOwner(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        CancellationToken cancellationToken = default
    )
    {
        const string operation = $"{nameof(FacilitiesController)}.{nameof(GetFacilityFromOwner)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["ownerId"] = ownerId,
                ["facilityId"] = facilityId,
            }
        );

        StartStopwatch();

        Result<Facility> result = await facilityHandler.GetFacilityFromOwner(
            ownerId,
            facilityId,
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpPost]
    [EndpointName("Create")]
    public async Task<ActionResult<Facility>> Create(
        [FromRoute] Guid ownerId,
        [FromBody] CreateFacilityViewModel request,
        CancellationToken cancellationToken = default
    )
    {
        const string operation = $"{nameof(FacilitiesController)}.{nameof(Create)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object> { ["operation"] = operation, ["ownerId"] = ownerId }
        );

        StartStopwatch();

        Result<Facility> result = await facilityHandler.Create(
            new CreateFacilityDTO()
            {
                OwnerId = ownerId,
                Name = request.Name,
                Address = request.Address,
                City = request.City,
                State = request.State,
                ZipCode = request.ZipCode,
                RegistrationNumber = request.RegistrationNumber,
            },
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpPut("{facilityId}")]
    [EndpointName("Update")]
    public async Task<ActionResult<Facility>> Update(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromBody] UpdateFacilityViewModel facility,
        CancellationToken cancellationToken = default
    )
    {
        const string operation = $"{nameof(FacilitiesController)}.{nameof(Update)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["ownerId"] = ownerId,
                ["facilityId"] = facilityId,
            }
        );

        StartStopwatch();

        Result<Facility> result = await facilityHandler.Update(
            new UpdateFacilityDTO()
            {
                Id = facilityId,
                OwnerId = ownerId,
                Name = facility.Name,
                Address = facility.Address,
                City = facility.City,
                State = facility.State,
                ZipCode = facility.ZipCode,
                RegistrationNumber = facility.RegistrationNumber,
            },
            cancellationToken
        );

        return HandleResult(result);
    }

    [HttpDelete("{facilityId}")]
    [EndpointName("Delete")]
    public async Task<ActionResult<bool>> Delete(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        CancellationToken cancellationToken = default
    )
    {
        const string operation = $"{nameof(FacilitiesController)}.{nameof(Delete)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object>
            {
                ["operation"] = operation,
                ["ownerId"] = ownerId,
                ["facilityId"] = facilityId,
            }
        );

        StartStopwatch();

        Result<bool> result = await facilityHandler.Delete(
            new DeleteFacilityDTO() { OwnerId = ownerId, FacilityId = facilityId },
            cancellationToken
        );

        return HandleResult(result);
    }
}
