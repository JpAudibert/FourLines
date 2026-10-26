using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;

namespace FourLines.Tests.Concurrency.Seed;

public static class SportSeed
{
    public static readonly Sport Default = new()
    {
        Name = "Test Sport",
        Indoor = true,
        StartingPlayersCount = 5,
        MaxPlayersCount = 10,
        HasFixedPosition = true,
        FixedPositionQuantity = 2,
    };

    public static readonly Sport WithoutGoalkeeper = new()
    {
        Name = "Test Sport 2",
        Indoor = true,
        StartingPlayersCount = 5,
        MaxPlayersCount = 10,
        HasFixedPosition = false,
        FixedPositionQuantity = 0,
    };

    public static async Task SeedAsync(FourLinesContext context)
    {
        await context.Sports.AddRangeAsync(Default, WithoutGoalkeeper);

        await context.SaveChangesAsync();
    }
}
