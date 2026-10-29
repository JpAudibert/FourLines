using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;

namespace FourLines.Tests.Shared.Seed;

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

    public static readonly Court WithNoSchedule = new()
    {
        FacilityId = FacilitySeed.NoSchedules.Id,
        SportId = SportSeed.Default.Id,
        Name = "Test Court 2",
        IsActive = true,
        DefaultPrice = new Money(50.00m, "BRL"),
        MaintenancePeriodInMinutes = 0,
        RentingPeriodInMinutes = 60,
    };

    public static readonly Court CourtWithNoSchedule2 = new()
    {
        FacilityId = FacilitySeed.Default2.Id,
        SportId = SportSeed.Default.Id,
        Name = "Test Court 2",
        IsActive = true,
        DefaultPrice = new Money(50.00m, "BRL"),
        MaintenancePeriodInMinutes = 0,
        RentingPeriodInMinutes = 60,
    };

    public static readonly Court ToBeUpdated = new()
    {
        FacilityId = FacilitySeed.Default.Id,
        SportId = SportSeed.Default.Id,
        Name = "To be updated",
        IsActive = true,
        DefaultPrice = new Money(50.00m, "BRL"),
        MaintenancePeriodInMinutes = 0,
        RentingPeriodInMinutes = 60,
    };

    public static readonly Court ToBeDeleted = new()
    {
        FacilityId = FacilitySeed.Default.Id,
        SportId = SportSeed.Default.Id,
        Name = "To be deleted",
        IsActive = true,
        DefaultPrice = new Money(50.00m, "BRL"),
        MaintenancePeriodInMinutes = 0,
        RentingPeriodInMinutes = 60,
    };

    public static readonly Court Court2 = new()
    {
        FacilityId = FacilitySeed.Default.Id,
        SportId = SportSeed.Default.Id,
        Name = "Test Court 2",
        IsActive = true,
        DefaultPrice = new Money(50.00m, "BRL"),
        MaintenancePeriodInMinutes = 0,
        RentingPeriodInMinutes = 60,
    };
    public static readonly Court Court3 = new()
    {
        FacilityId = FacilitySeed.Dummy.Id,
        SportId = SportSeed.WithoutGoalkeeper.Id,
        Name = "Test Court 3",
        IsActive = true,
        DefaultPrice = new Money(50.00m, "BRL"),
        MaintenancePeriodInMinutes = 0,
        RentingPeriodInMinutes = 60,
    };
    public static readonly Court CourtWithSportWithoutGoalkeeper = new()
    {
        FacilityId = FacilitySeed.Default.Id,
        SportId = SportSeed.WithoutGoalkeeper.Id,
        Name = "Test Court 4",
        IsActive = true,
        DefaultPrice = new Money(50.00m, "BRL"),
        MaintenancePeriodInMinutes = 0,
        RentingPeriodInMinutes = 60,
    };

    public static async Task SeedAsync(FourLinesContext context)
    {
        await context.Courts.AddRangeAsync(
            Default,
            WithNoSchedule,
            CourtWithNoSchedule2,
            ToBeUpdated,
            ToBeDeleted,
            CourtWithSportWithoutGoalkeeper
        );

        await context.SaveChangesAsync();
    }
}
