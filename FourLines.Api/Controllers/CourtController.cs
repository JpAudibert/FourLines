namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Authorize(Roles = $"{RoleConstants.FacilityOwner}, {RoleConstants.Admin}")]
[Route("api/v{version:apiVersion}/owner/{ownerId}/facility/{facilityId}/[controller]")]
public class CourtController(CourtHandler courtHandler) : ControllerBase
{
    private readonly CourtHandler _courtHandler = courtHandler;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId)
    {
        Result<IEnumerable<Court>> result = await _courtHandler.GetAllCourtsFromFacility(ownerId, facilityId);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("{courtId}")]
    public async Task<IActionResult> GetById(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromRoute] Guid courtId)
    {
        Result<Court> result = await _courtHandler.GetFacility(ownerId, facilityId, courtId);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<Court>> Create(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromBody] CreateCourtViewModel newCourt)
    {
        Result<Court> result = await _courtHandler.Create(new CreateCourtDTO()
        {
            OwnerId = ownerId,
            FacilityId = facilityId,
            SportId = newCourt.SportId,
            Name = newCourt.Name,
            IsActive = newCourt.IsActive,
        });

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPut("{courtId}")]
    public async Task<ActionResult<Court>> Update(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromRoute] Guid courtId,
        [FromBody] UpdateCourtViewModel updateCourt)
    {
        Result<Court> result = await _courtHandler.Update(new UpdateCourtDTO()
        {
            Id = courtId,
            OwnerId = ownerId,
            FacilityId = facilityId,
            SportId = updateCourt.SportId,
            Name = updateCourt.Name,
            IsActive = updateCourt.IsActive,
        });

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpDelete("{courtId}")]
    public async Task<ActionResult<bool>> Delete(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromRoute] Guid courtId)
    {
        Result<bool> result = await _courtHandler.Delete(ownerId, facilityId, courtId);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
