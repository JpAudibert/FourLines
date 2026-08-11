using FourLines.Domain.Constants;
using FourLines.Domain.Models;

namespace FourLines.Tests.Shared;

public class InMemoryDataSource
{
    public static DateTimeOffset dateTime = DateTimeOffset.Now;
    public static Role roleOwner = new() { Name = RoleConstants.FacilityOwner };
    public static Role rolePlayer = new() { Name = RoleConstants.Player };

    public static User userOwner = new()
    {
        RoleId = roleOwner.Id,
        Name = "John Doe",
        Email = "john.doe@example.com",
        PasswordHash = "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1970, 1, 1),
        Phone = "55 54 9 9999-9999",
        RegistrationNumber = "383.975.210-89",
    };

    public static User userPlayer = new()
    {
        RoleId = rolePlayer.Id,
        Name = "Jane Smith",
        Email = "jane.smith@example.com",
        PasswordHash = "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
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

    public static FacilitySchedule facilitySchedule1 = new()
    {
        FacilityId = facility1.Id,
        DayOfWeek = DayOfWeek.Tuesday,
        OpensAt = new TimeOnly(8, 0),
        ClosesAt = new TimeOnly(20, 0),
    };

    public static FacilitySchedule facilitySchedule2 = new()
    {
        FacilityId = facility1.Id,
        DayOfWeek = DayOfWeek.Thursday,
        OpensAt = new TimeOnly(8, 0),
        ClosesAt = new TimeOnly(20, 0),
    };

    public static Reservation reservation1 = new()
    {
        CourtId = court1.Id,
        UserId = userPlayer.Id,
        Period = new TimeRange(dateTime, dateTime.AddHours(1)),
        Status = ReservationStatus.Pending,
    };

    public static Reservation reservation2 = new()
    {
        CourtId = court1.Id,
        UserId = userPlayer.Id,
        Period = new TimeRange(dateTime.AddHours(1), dateTime.AddHours(2)),
        Status = ReservationStatus.Pending,
    };

    public static Reservation reservation3 = new()
    {
        CourtId = court2.Id,
        UserId = userPlayer.Id,
        Period = new TimeRange(dateTime.AddHours(2), dateTime.AddHours(3)),
        Status = ReservationStatus.Pending,
    };
}
