using Microsoft.AspNetCore.Mvc;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class FacilityController : Controller
{
    private readonly IStandardRepository<Facility> _repository;

    public FacilityController(IStandardRepository<Facility> repository)
    {
        _repository = repository;
    }

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Facility facility)
    {
        await _repository.AddAsync(facility);

        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Facility facility)
    {
        if (id != facility.Id)
            return BadRequest();

        Facility? existingFacility = await _repository.GetEntityAsync(id);

        if (existingFacility is null)
            return NotFound(id);

        await _repository.UpdateAsync(facility);

        return Ok(id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        Facility? existingFacility = await _repository.GetEntityAsync(id);

        if (existingFacility is null)
            return NotFound(id);

        await _repository.DeleteAsync(id);

        return Ok(id);
    }
}
