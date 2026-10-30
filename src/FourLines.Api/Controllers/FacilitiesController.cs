using FourLines.Application.Interfaces;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/[controller]")]
public class FacilitiesController(
    ILogger<FacilitiesController> logger,
    IFacilityHandler facilityHandler,
    ICurrentUserProvider currentUserProvider
) : ApiControllerBase(logger, currentUserProvider)
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

    [HttpGet("mine")]
    [EndpointName("GetAllFromOwner")]
    [Authorize(Roles = $"{RoleConstants.FacilityOwner}, {RoleConstants.Admin}")]
    public async Task<ActionResult<IEnumerable<Facility>>> GetAllFromOwner(
        CancellationToken cancellationToken = default
    )
    {
        if (!TryGetCurrentUser(out Guid ownerId, out bool isAdmin))
            return Unauthorized();

        const string operation = $"{nameof(FacilitiesController)}.{nameof(GetAllFromOwner)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object> { ["operation"] = operation, ["ownerId"] = ownerId }
        );

        StartStopwatch();

        Result<IEnumerable<Facility>> result = await facilityHandler.GetFacilitiesFromOwner(
            ownerId,
            cancellationToken,
            isAdmin
        );

        return HandleResult(result);
    }

    [HttpGet("{facilityId:guid}")]
    [EndpointName("GetFacilityFromOwner")]
    [Authorize(Roles = $"{RoleConstants.FacilityOwner}, {RoleConstants.Admin}")]
    public async Task<ActionResult<Facility>> GetFacilityFromOwner(
        [FromRoute] Guid facilityId,
        CancellationToken cancellationToken = default
    )
    {
        if (!TryGetCurrentUser(out Guid ownerId, out bool isAdmin))
            return Unauthorized();

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
            cancellationToken,
            isAdmin
        );

        return HandleResult(result);
    }

    [HttpPost]
    [EndpointName("Create")]
    [Authorize(Roles = $"{RoleConstants.FacilityOwner}, {RoleConstants.Admin}")]
    public async Task<ActionResult<Facility>> Create(
        [FromBody] CreateFacilityRequest request,
        CancellationToken cancellationToken = default
    )
    {
        if (!TryGetCurrentUser(out Guid ownerId, out bool isAdmin))
            return Unauthorized();

        const string operation = $"{nameof(FacilitiesController)}.{nameof(Create)}";
        using var scope = logger.BeginScope(
            new Dictionary<string, object> { ["operation"] = operation, ["ownerId"] = ownerId }
        );

        StartStopwatch();

        Result<Facility> result = await facilityHandler.Create(
            new CreateFacilityDTO()
            {
                OwnerId = ownerId,
                IsAdmin = isAdmin,
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
    [Authorize(Roles = $"{RoleConstants.FacilityOwner}, {RoleConstants.Admin}")]
    public async Task<ActionResult<Facility>> Update(
        [FromRoute] Guid facilityId,
        [FromBody] UpdateFacilityRequest facility,
        CancellationToken cancellationToken = default
    )
    {
        if (!TryGetCurrentUser(out Guid ownerId, out bool isAdmin))
            return Unauthorized();

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
                IsAdmin = isAdmin,
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
    [Authorize(Roles = $"{RoleConstants.FacilityOwner}, {RoleConstants.Admin}")]
    public async Task<ActionResult<bool>> Delete(
        [FromRoute] Guid facilityId,
        CancellationToken cancellationToken = default
    )
    {
        if (!TryGetCurrentUser(out Guid ownerId, out bool isAdmin))
            return Unauthorized();

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
            new DeleteFacilityDTO()
            {
                OwnerId = ownerId,
                IsAdmin = isAdmin,
                FacilityId = facilityId,
            },
            cancellationToken
        );

        return HandleResult(result);
    }
}
