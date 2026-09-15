using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;

namespace FourLines.Tests.Concurrency.Seed;

public static class CourtSeed
{
    public static readonly Court Default = new()
    {
        FacilityId = FacilitySeed.Default.Id,
        SportId = SportSeed.Default.Id,
        Name = "Test Court",
        IsActive = true,
        DefaultPrice = new Money(50.00m, "BRL"),
        MaintenancePeriodInMinutes = 0,
        RentingPeriodInMinutes = 60,
    };

    public static async Task SeedAsync(FourLinesContext context)
    {
        await context.Courts.AddRangeAsync(Default);

        await context.SaveChangesAsync();
    }
}
