namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
//[Authorize(Roles = $"{RoleConstants.Admin}")]
[ExcludeFromCodeCoverage]
public class SeederController(ILogger<SeederController> logger, SeederHandler seederHandler) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> SeedDatabase(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Seeding started");
        await seederHandler.Seed(cancellationToken);
        logger.LogInformation("Seeding completed");

        return Ok("Ok");
    }
}
