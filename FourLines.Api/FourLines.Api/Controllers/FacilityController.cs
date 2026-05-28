using FourLines.Api.Interfaces;
using FourLines.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FourLines.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class FacilityController : Controller
{
    private readonly IStandardRepository<Facility> _repository;

    public FacilityController(IStandardRepository<Facility> repository)
    {
        _repository = repository;
    }

    [HttpPost("insert")]
    public async Task<IActionResult> Insert([FromBody] Facility facility)
    {
        facility.CreatedAt = DateTimeOffset.UtcNow;
        facility.UpdatedAt = facility.CreatedAt;

        await _repository.AddAsync(facility);

        return Ok(facility);
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Facility facility)
    {
        if (id != facility.Id)
        {
            return BadRequest();
        }

        Facility? existingFacility = await _repository.GetEntityAsync(id);

        if (existingFacility is null)
        {
            return NotFound();
        }

        facility.CreatedAt = existingFacility.CreatedAt;
        facility.UpdatedAt = DateTimeOffset.UtcNow;

        await _repository.UpdateAsync(facility);

        return NoContent();
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        Facility? existingFacility = await _repository.GetEntityAsync(id);

        if (existingFacility is null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);

        return NoContent();
    }
}