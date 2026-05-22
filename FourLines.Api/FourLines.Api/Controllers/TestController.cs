using FourLines.Api.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace FourLines.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : Controller
{
    private readonly FourLinesContext _context;

    public TestController(FourLinesContext context)
    {
        _context = context;
    }

    [HttpGet("/test")]
    public IActionResult Test()
    {
        return Ok(_context.Users);
    }
}
