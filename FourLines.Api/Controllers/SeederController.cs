using FourLines.Application.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class SeederController(SeederHandler seederHandler) : ControllerBase
{
    private readonly SeederHandler _seederHandler = seederHandler;

    [HttpPost]
    public async Task<ActionResult> SeedDatabase()
    {
        await _seederHandler.Seed();

        return Ok("Ok");
    }
}
