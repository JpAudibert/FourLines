using FourLines.Domain.Models;
using FourLines.Infrastructure.Contexts;

namespace FourLines.Tests.Shared.Seed;

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

    public static readonly Facility Default2 = new()
    {
        Name = "Default Facility 2",
        Address = "456 Test Ave",
        City = "Test City 2",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "0987654458",
        OwnerId = UserSeed.Owner.Id,
    };

    public static readonly Facility NoSchedules = new()
    {
        Name = "No schedules facility",
        Address = "789 Test Blvd",
        City = "Test City",
        State = "TS",
        ZipCode = "12346",
        RegistrationNumber = "0994654389",
        OwnerId = UserSeed.Owner.Id,
    };

    public static readonly Facility NoSchedules2 = new()
    {
        Name = "No schedules facility",
        Address = "789 Test Blvd",
        City = "Test City",
        State = "TS",
        ZipCode = "12346",
        RegistrationNumber = "0994874389",
        OwnerId = UserSeed.Owner.Id,
    };

    public static readonly Facility ToBeUpdated = new()
    {
        Name = "To be updated",
        Address = "456 Test Ave",
        City = "Test City 2",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "0987654987",
        OwnerId = UserSeed.Owner.Id,
    };

    public static readonly Facility ToBeDeleted = new()
    {
        Name = "To be deleted",
        Address = "456 Test Ave",
        City = "Test City 2",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "0986324987",
        OwnerId = UserSeed.Owner.Id,
    };

    public static readonly Facility Dummy = new()
    {
        Name = "Dummy Facility",
        Address = "789 Test Blvd",
        City = "Test City 3",
        State = "TS",
        ZipCode = "12346",
        RegistrationNumber = "0987654345",
        OwnerId = UserSeed.Owner.Id,
    };

    public static async Task SeedAsync(FourLinesContext context)
    {
        Guid ownerId = UserSeed.Owner.Id;
        await context.Facilities.AddRangeAsync(
            Default,
            Default2,
            NoSchedules,
            NoSchedules2,
            ToBeUpdated,
            ToBeDeleted,
            Dummy
        );

        await context.SaveChangesAsync();
    }
}
