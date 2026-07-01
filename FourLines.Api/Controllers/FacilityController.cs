using FourLines.Api.ViewModels.Facilities;
using FourLines.Application.DTOs.Facilities;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class FacilityController(FacilityHandler facilityHandler, IStandardRepository<Facility> repository) : Controller
{
    private readonly FacilityHandler _facilityHandler = facilityHandler;
    private readonly IStandardRepository<Facility> _repository = repository;

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Facility> facilities = await _repository.GetAllAsync();

        return Ok(facilities);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        Facility? facility = await _repository.GetEntityAsync(id);

        if (facility is null)
            return NotFound();

        return Ok(facility);
    }

    [HttpPost("{ownerId}")]
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

    //[HttpPut("{id}")]
    //public async Task<IActionResult> Update(Guid id, [FromBody] Facility facility)
    //{
    //    if (id != facility.Id)
    //        return BadRequest();

    //    Facility? existingFacility = await _repository.GetEntityAsync(id);

    //    if (existingFacility is null)
    //        return NotFound(id);

    //    await _repository.UpdateAsync(facility);

    //    return Ok(id);
    //}

    //[HttpDelete("{id}")]
    //public async Task<IActionResult> Delete(Guid id)
    //{
    //    Facility? existingFacility = await _repository.GetEntityAsync(id);

    //    if (existingFacility is null)
    //        return NotFound(id);

    //    await _repository.DeleteAsync(id);

    //    return Ok(id);
    //}
}
