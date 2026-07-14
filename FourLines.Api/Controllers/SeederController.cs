namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
//[Authorize(Roles = $"{RoleConstants.Admin}")]
[ExcludeFromCodeCoverage]
public class SeederController(ILogger<SeederController> logger, SeederHandler seederHandler) : ControllerBase
{
    private readonly ILogger<SeederController> _logger = logger;
    private readonly SeederHandler _seederHandler = seederHandler;

    [HttpPost]
    public async Task<ActionResult> SeedDatabase()
    {
        _logger.LogInformation("Seeding started");
        await _seederHandler.Seed();
        _logger.LogInformation("Seeding completed");

        return Ok("Ok");
    }
}
