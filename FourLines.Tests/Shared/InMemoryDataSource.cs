using FourLines.Domain.Constants;
using FourLines.Domain.Models;

namespace FourLines.Tests.Shared;

public class InMemoryDataSource
{
    public static Role roleOwner = new() { Name = RoleConstants.FacilityOwner };
    public static Role rolePlayer = new() { Name = RoleConstants.Player };

    public static User userOwner = new()
    {
        RoleId = roleOwner.Id,
        Name = "John Doe",
        Email = "john.doe@example.com",
        PasswordHash = "Password123!",
        Birthday = new DateOnly(1970, 1, 1),
        Phone = "55 54 9 9999-9999",
        RegistrationNumber = "383.975.210-89",
    };

    public static User userPlayer = new()
    {
        RoleId = rolePlayer.Id,
        Name = "Jane Smith",
        Email = "jane.smith@example.com",
        PasswordHash = "Password123!",
        Birthday = new DateOnly(1970, 1, 1),
        Phone = "55 54 9 9999-9999",
        RegistrationNumber = "383.975.211-89",
    };

    public static Facility facility1 = new()
    {
        Name = "Test Facility 1",
        Address = "123 Test St",
        City = "Test City",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "1234567890",
        OwnerId = userOwner.Id,
    };

    public static Facility facility2 = new()
    {
        Name = "Test Facility 2",
        Address = "456 Test Ave",
        City = "Test City 2",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "0987654321",
        OwnerId = userOwner.Id,
    };

    public static Sport sport = new()
    {
        Name = "Test Sport",
        Indoor = true,
        StartingPlayersCount = 5,
        MaxPlayersCount = 10,
    };

    public static Court court1 = new()
    {
        FacilityId = facility1.Id,
        SportId = sport.Id,
        Name = "Test Court",
        IsActive = true,
    };

    public static Court court2 = new()
    {
        FacilityId = facility1.Id,
        SportId = sport.Id,
        Name = "Test Court 2",
        IsActive = true,
    };

}