using Microsoft.AspNetCore.Mvc;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class UserController : Controller
{
    private readonly IStandardRepository<User> _repository;

    public UserController(IStandardRepository<User> repository)
    {
        _repository = repository;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<User> users = await _repository.GetAllAsync();

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        User? user = await _repository.GetEntityAsync(id);

        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] User user)
    {
        await _repository.AddAsync(user);

        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] User user)
    {
        if (id != user.Id)
            return BadRequest();

        User? existingUser = await _repository.GetEntityAsync(id);

        if (existingUser is null)
            return NotFound(id);

        await _repository.UpdateAsync(user);

        return Ok(id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        User? existingUser = await _repository.GetEntityAsync(id);

        if (existingUser is null)
            return NotFound(id);

        await _repository.DeleteAsync(id);

        return Ok(id);
    }
}
