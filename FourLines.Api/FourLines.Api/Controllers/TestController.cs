using FourLines.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class TestController(TestService service) : Controller
{
    private readonly TestService _service = service;

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        Role roles = await _service.GetSomething();

        return Ok(roles);
    }
}
