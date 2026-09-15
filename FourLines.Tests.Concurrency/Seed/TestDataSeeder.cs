using FourLines.Infrastructure.Contexts;

namespace FourLines.Tests.Concurrency.Seed;

public static class TestDataSeeder
{
    public static async Task SeedAsync(FourLinesContext context)
    {
        await RoleSeed.SeedAsync(context);
        await UserSeed.SeedAsync(context);
        await SportSeed.SeedAsync(context);
        await FacilitySeed.SeedAsync(context);
        await CourtSeed.SeedAsync(context);
        await FacilityScheduleSeed.SeedAsync(context);
    }
}
