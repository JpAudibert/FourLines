using FourLines.Api.Contexts;
using FourLines.Api.Interfaces;
using FourLines.Api.Models;
using FourLines.Api.Seeders;
using Microsoft.AspNetCore.Mvc;

namespace FourLines.Api.Controllers;

[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class SeederController : Controller
{
    private readonly CourtSeeder _courtSeeder = new();
    private readonly UserSeeder _userSeeder = new();
    private readonly FacilitySeeder _facilitySeeder = new();
    private readonly RoleSeeder _roleSeeder = new();
    private readonly SportSeeder _sportSeeder = new();
    private readonly FourLinesContext _context;
    private readonly IWebHostEnvironment _environment;

    public SeederController(FourLinesContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpPost]
    public async Task<IActionResult> Create()
    {
        if (!_environment.IsDevelopment())
            return BadRequest("Seeding is only allowed in development environment.");

        await _roleSeeder.SeedAsync(_context);
        await _userSeeder.SeedAsync(_context);
        await _sportSeeder.SeedAsync(_context);
        await _courtSeeder.SeedAsync(_context);
        await _facilitySeeder.SeedAsync(_context);

        return Created();
    }

}