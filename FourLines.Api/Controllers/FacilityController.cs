using FourLines.Api.ViewModels.Facilities;
using FourLines.Application.DTOs.Facilities;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}")]
public class FacilityController(FacilityHandler facilityHandler, IStandardRepository<Facility> repository) : Controller
{
    private readonly FacilityHandler _facilityHandler = facilityHandler;
    private readonly IStandardRepository<Facility> _repository = repository;

    //[HttpGet("facilities")]
    //[EndpointName("GetAll")]
    //public async Task<IActionResult> GetAll()
    //{
    //    IEnumerable<Facility> facilities = await _repository.GetAllAsync();

    //    return Ok(facilities);
    //}

    [HttpGet("owner/{ownerId}")]
    [EndpointName("GetAllFromOwner")]
    public async Task<ActionResult<IEnumerable<Facility>>> GetAllFromOwner([FromRoute] Guid ownerId)
    {
        Result<IEnumerable<Facility>> result = await _facilityHandler.GetFacilitiesFromOwner(ownerId);

        if (result.IsFailure)
            BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpGet("owner/{ownerId}/facility/{facilityId}")]
    [EndpointName("GetFacilityFromOwner")]
    public async Task<IActionResult> GetFacilityFromOwner([FromRoute] Guid ownerId, [FromRoute] Guid facilityId)
    {
        Result<Facility> result = await _facilityHandler.GetFacilityFromOwner(ownerId, facilityId);

        if (result.IsFailure)
            BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost("owner/{ownerId}")]
    [EndpointName("Create")]
    public async Task<ActionResult<Facility>> Create([FromRoute] Guid ownerId, [FromBody] CreateFacilityViewModel request)
    {
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

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPut("owner/{ownerId}/facility/{facilityId}")]
    [EndpointName("Update")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid ownerId,
        [FromRoute] Guid facilityId,
        [FromBody] UpdateFacilityViewModel facility)
    {
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

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpDelete("owner/{ownerId}/facility/{facilityId}")]
    [EndpointName("Delete")]
    public async Task<IActionResult> Delete([FromRoute] Guid ownerId, [FromRoute] Guid facilityId)
    {
        Result<bool> result = await _facilityHandler.Delete(ownerId, facilityId);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }
}
