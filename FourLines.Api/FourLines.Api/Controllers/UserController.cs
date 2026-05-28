using FourLines.Api.Interfaces;
using FourLines.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace FourLines.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly IStandardRepository<User> _repository;

    public UserController(IStandardRepository<User> repository)
    {
        _repository = repository;
    }

    [HttpPost("insert")]
    public async Task<IActionResult> Insert([FromBody] User user)
    {
        user.CreatedAt = DateTimeOffset.UtcNow;
        user.UpdatedAt = user.CreatedAt;

        await _repository.AddAsync(user);

        return Ok(user);
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        User? existingUser = await _repository.GetEntityAsync(id);

        if (existingUser is null)
        {
            return NotFound();
        }

        user.CreatedAt = existingUser.CreatedAt;
        user.UpdatedAt = DateTimeOffset.UtcNow;

        await _repository.UpdateAsync(user);

        return NoContent();
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        User? existingUser = await _repository.GetEntityAsync(id);

        if (existingUser is null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);

        return NoContent();
    }
}