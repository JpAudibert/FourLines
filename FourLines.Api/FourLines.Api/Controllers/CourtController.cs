using FourLines.Api.Interfaces;
using FourLines.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class CourtController : Controller
{
    private readonly IStandardRepository<Court> _repository;

    public CourtController(IStandardRepository<Court> repository)
    {
        _repository = repository;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Court> courts = await _repository.GetAllAsync();

        return Ok(courts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        Court? court = await _repository.GetEntityAsync(id);

        if (court is null)
            return NotFound();

        return Ok(court);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Court court)
    {
        await _repository.AddAsync(court);

        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Court court)
    {
        if (id != court.Id)
            return BadRequest();

        Court? existingCourt = await _repository.GetEntityAsync(id);

        if (existingCourt is null)
            return NotFound(id);

        await _repository.UpdateAsync(court);

        return Ok(id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        Court? existingCourt = await _repository.GetEntityAsync(id);

        if (existingCourt is null)
            return NotFound(id);

        await _repository.DeleteAsync(id);

        return Ok(id);
    }
}