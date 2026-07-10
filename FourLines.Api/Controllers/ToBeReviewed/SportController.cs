//namespace FourLines.Api.Controllers;

//[ApiVersion("1")]
//[ApiController]
//[Route("api/v{version:apiVersion}/[controller]")]
//public class SportController : Controller
//{
//    private readonly IStandardRepository<Sport> _repository;

//    public SportController(IStandardRepository<Sport> repository)
//    {
//        _repository = repository;
//    }

//    [HttpGet()]
//    public async Task<IActionResult> GetAll()
//    {
//        IEnumerable<Sport> sports = await _repository.GetAllAsync();

//        return Ok(sports);
//    }

//    [HttpGet("{id}")]
//    public async Task<IActionResult> GetById(Guid id)
//    {
//        Sport? sport = await _repository.GetEntityAsync(id);

//        if (sport is null)
//            return NotFound();

//        return Ok(sport);
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create([FromBody] Sport sport)
//    {
//        await _repository.AddAsync(sport);

//        return Created();
//    }

//    [HttpPut("{id}")]
//    public async Task<IActionResult> Update(Guid id, [FromBody] Sport sport)
//    {
//        if (id != sport.Id)
//            return BadRequest();

//        Sport? existingSport = await _repository.GetEntityAsync(id);

//        if (existingSport is null)
//            return NotFound(id);

//        await _repository.UpdateAsync(sport);

//        return Ok(id);
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> Delete(Guid id)
//    {
//        Sport? existingSport = await _repository.GetEntityAsync(id);

//        if (existingSport is null)
//            return NotFound(id);

//        await _repository.DeleteAsync(id);

//        return Ok(id);
//    }
//}
