using FourLines.Api.Interfaces;
using FourLines.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FourLines.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CourtController : Controller
{
    private readonly IStandardRepository<Court> _repository;

    public CourtController(IStandardRepository<Court> repository)
    {
        _repository = repository;
    }

    [HttpPost("insert")]
    public async Task<IActionResult> Insert([FromBody] Court court)
    {
        court.CreatedAt = DateTimeOffset.UtcNow;
        court.UpdatedAt = court.CreatedAt;

        await _repository.AddAsync(court);

        return Ok(court);
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Court court)
    {
        if (id != court.Id)
        {
            return BadRequest();
        }

        Court? existingCourt = await _repository.GetEntityAsync(id);

        if (existingCourt is null)
        {
            return NotFound();
        }

        court.CreatedAt = existingCourt.CreatedAt;
        court.UpdatedAt = DateTimeOffset.UtcNow;

        await _repository.UpdateAsync(court);

        return NoContent();
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        Court? existingCourt = await _repository.GetEntityAsync(id);

        if (existingCourt is null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);

        return NoContent();
    }
}