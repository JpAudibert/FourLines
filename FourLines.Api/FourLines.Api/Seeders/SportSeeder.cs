using FourLines.Api.Contexts;
using FourLines.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FourLines.Api.Seeders;

public sealed class SportSeeder : ISeeder
{
    public async Task SeedAsync(FourLinesContext context)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;

        var sports = new[]
        {
            new Sport { Name = "Futebol", Indoor = false, StartingPlayersCount = 5, MaxPlayersCount = 10, CreatedAt = now, UpdatedAt = now },
            new Sport { Name = "Futsal", Indoor = true, StartingPlayersCount = 5, MaxPlayersCount = 10, CreatedAt = now, UpdatedAt = now },
            new Sport { Name = "Volei", Indoor = true, StartingPlayersCount = 6, MaxPlayersCount = 12, CreatedAt = now, UpdatedAt = now },
            new Sport { Name = "Basquete", Indoor = true, StartingPlayersCount = 5, MaxPlayersCount = 12, CreatedAt = now, UpdatedAt = now }
        };

        foreach (var sport in sports)
        {
            bool exists = await context.Sports.AnyAsync(existingSport => existingSport.Name == sport.Name);

            if (!exists)
            {
                await context.Sports.AddAsync(sport);
            }
        }

        await context.SaveChangesAsync();
    }
}