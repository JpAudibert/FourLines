using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;

namespace FourLines.Tests.Shared.Seed;

public static class FacilityScheduleSeed
{
    public static readonly FacilitySchedule ToBeDeleted = new()
    {
        FacilityId = FacilitySeed.NoSchedules.Id,
        DayOfWeek = DayOfWeek.Thursday,
        OpensAt = new TimeOnly(8, 0),
        ClosesAt = new TimeOnly(20, 0),
    };

    public static readonly FacilitySchedule ToBeUpdated = new()
    {
        FacilityId = FacilitySeed.NoSchedules.Id,
        DayOfWeek = DayOfWeek.Friday,
        OpensAt = new TimeOnly(0, 0),
        ClosesAt = new TimeOnly(23, 59),
    };

    public static async Task SeedAsync(FourLinesContext context)
    {
        var schedules = Enum.GetValues<DayOfWeek>()
            .Select(day => new FacilitySchedule
            {
                FacilityId = FacilitySeed.Default.Id,
                DayOfWeek = day,
                OpensAt = new TimeOnly(0, 0),
                ClosesAt = new TimeOnly(23, 59),
            })
            .ToList();

        await context.FacilitySchedules.AddRangeAsync(schedules);
        await context.FacilitySchedules.AddRangeAsync(ToBeUpdated, ToBeDeleted);

        await context.SaveChangesAsync();
    }
}
