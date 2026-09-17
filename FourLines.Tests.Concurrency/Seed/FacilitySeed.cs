using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;

namespace FourLines.Tests.Concurrency.Seed;

public static class FacilitySeed
{
    public static readonly Facility Default = new()
    {
        Name = "Default Facility 1",
        Address = "123 Test St",
        City = "Test City",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "1234567890",
        OwnerId = UserSeed.Owner.Id,
    };

    public static async Task SeedAsync(FourLinesContext context)
    {
        await context.Facilities.AddRangeAsync(Default);

        await context.SaveChangesAsync();
    }
}
