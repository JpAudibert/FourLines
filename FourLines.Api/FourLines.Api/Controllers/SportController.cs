using FourLines.Api.Interfaces;
using FourLines.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FourLines.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SportController : Controller
{
    private readonly IStandardRepository<Sport> _repository;

    public SportController(IStandardRepository<Sport> repository)
    {
        _repository = repository;
    }

    [HttpPost("insert")]
    public async Task<IActionResult> Insert([FromBody] Sport sport)
    {
        sport.CreatedAt = DateTimeOffset.UtcNow;
        sport.UpdatedAt = sport.CreatedAt;

        await _repository.AddAsync(sport);

        return Ok(sport);
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Sport sport)
    {
        if (id != sport.Id)
        {
            return BadRequest();
        }

        Sport? existingSport = await _repository.GetEntityAsync(id);

        if (existingSport is null)
        {
            return NotFound();
        }

        sport.CreatedAt = existingSport.CreatedAt;
        sport.UpdatedAt = DateTimeOffset.UtcNow;

        await _repository.UpdateAsync(sport);

        return NoContent();
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        Sport? existingSport = await _repository.GetEntityAsync(id);

        if (existingSport is null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);

        return NoContent();
    }
}