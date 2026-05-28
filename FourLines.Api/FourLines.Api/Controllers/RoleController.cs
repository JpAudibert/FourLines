using FourLines.Api.Interfaces;
using FourLines.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FourLines.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class RoleController : Controller
{
    private readonly IStandardRepository<Role> _repository;

    public RoleController(IStandardRepository<Role> repository)
    {
        _repository = repository;
    }

    [HttpPost("insert")]
    public async Task<IActionResult> Insert([FromBody] Role role)
    {
        role.CreatedAt = DateTimeOffset.UtcNow;
        role.UpdatedAt = role.CreatedAt;

        await _repository.AddAsync(role);

        return Ok(role);
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Role role)
    {
        if (id != role.Id)
        {
            return BadRequest();
        }

        Role? existingRole = await _repository.GetEntityAsync(id);

        if (existingRole is null)
        {
            return NotFound();
        }

        role.CreatedAt = existingRole.CreatedAt;
        role.UpdatedAt = DateTimeOffset.UtcNow;

        await _repository.UpdateAsync(role);

        return NoContent();
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        Role? existingRole = await _repository.GetEntityAsync(id);

        if (existingRole is null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);

        return NoContent();
    }
}