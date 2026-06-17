using Microsoft.AspNetCore.Mvc;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class RoleController : Controller
{
    private readonly IStandardRepository<Role> _repository;

    public RoleController(IStandardRepository<Role> repository)
    {
        _repository = repository;
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Role> roles = await _repository.GetAllAsync();

        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        Role? role = await _repository.GetEntityAsync(id);

        if (role is null)
            return NotFound();

        return Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Role role)
    {
        await _repository.AddAsync(role);

        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Role role)
    {
        if (id != role.Id)
            return BadRequest();

        Role? existingRole = await _repository.GetEntityAsync(id);

        if (existingRole is null)
            return NotFound(id);

        await _repository.UpdateAsync(role);

        return Ok(id);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        Role? existingRole = await _repository.GetEntityAsync(id);

        if (existingRole is null)
            return NotFound(id);

        await _repository.DeleteAsync(id);

        return Ok(id);
    }
}
