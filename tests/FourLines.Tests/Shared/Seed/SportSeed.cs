using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;

namespace FourLines.Tests.Shared.Seed;

public static class SportSeed
{
    public static readonly Sport Default = new()
    {
        Name = "Test Sport",
        Indoor = true,
        StartingPlayersCount = 5,
        MaxPlayersCount = 10,
        HasFixedPosition = true,
    };

    public static readonly Sport WithoutGoalkeeper = new()
    {
        Name = "Test Sport 2",
        Indoor = true,
        StartingPlayersCount = 5,
        MaxPlayersCount = 10,
        HasFixedPosition = false,
    };

    public static readonly Sport SportToShuffle = new()
    {
        Name = "Sport to shuffle players",
        Indoor = true,
        StartingPlayersCount = 4,
        MaxPlayersCount = 8,
        HasFixedPosition = true,
    };

    public static async Task SeedAsync(FourLinesContext context)
    {
        await context.Sports.AddRangeAsync(Default, WithoutGoalkeeper, SportToShuffle);

        await context.SaveChangesAsync();
    }
}
