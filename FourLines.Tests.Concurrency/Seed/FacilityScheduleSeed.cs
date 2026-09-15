using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;

namespace FourLines.Tests.Concurrency.Seed;

public static class FacilityScheduleSeed
{
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

        await context.SaveChangesAsync();
    }
}
