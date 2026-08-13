using FourLines.Domain.Constants;
using FourLines.Domain.Models;

namespace FourLines.Tests.Shared;

public class InMemoryDataSource
{
    public readonly static DateTimeOffset DateTime = DateTimeOffset.Now;
    public readonly static Role RoleOwner = new() { Name = RoleConstants.FacilityOwner };
    public readonly static Role RolePlayer = new() { Name = RoleConstants.Player };

    public readonly static User UserOwner = new()
    {
        RoleId = RoleOwner.Id,
        Name = "John Doe",
        Email = "john.doe@example.com",
        PasswordHash = "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1970, 1, 1),
        Phone = "55 54 9 9999-9999",
        RegistrationNumber = "383.975.210-89",
    };

    public readonly static User UserPlayer = new()
    {
        RoleId = RolePlayer.Id,
        Name = "Jane Smith",
        Email = "jane.smith@example.com",
        PasswordHash = "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1970, 1, 1),
        Phone = "55 54 9 9999-9999",
        RegistrationNumber = "383.975.211-89",
    };

    public readonly static Facility Facility1 = new()
    {
        Name = "Test Facility 1",
        Address = "123 Test St",
        City = "Test City",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "1234567890",
        OwnerId = UserOwner.Id,
    };

    public readonly static Facility Facility2 = new()
    {
        Name = "Test Facility 2",
        Address = "456 Test Ave",
        City = "Test City 2",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "0987654321",
        OwnerId = UserOwner.Id,
    };

    public readonly static Sport TestSport = new()
    {
        Name = "Test Sport",
        Indoor = true,
        StartingPlayersCount = 5,
        MaxPlayersCount = 10,
    };

    public readonly static Court Court1 = new()
    {
        FacilityId = Facility1.Id,
        SportId = TestSport.Id,
        Name = "Test Court",
        IsActive = true,
    };

    public readonly static Court Court2 = new()
    {
        FacilityId = Facility1.Id,
        SportId = TestSport.Id,
        Name = "Test Court 2",
        IsActive = true,
    };

    public readonly static FacilitySchedule FacilitySchedule1 = new()
    {
        FacilityId = Facility1.Id,
        DayOfWeek = DayOfWeek.Tuesday,
        OpensAt = new TimeOnly(8, 0),
        ClosesAt = new TimeOnly(20, 0),
    };

    public readonly static FacilitySchedule FacilitySchedule2 = new()
    {
        FacilityId = Facility1.Id,
        DayOfWeek = DayOfWeek.Thursday,
        OpensAt = new TimeOnly(8, 0),
        ClosesAt = new TimeOnly(20, 0),
    };

    public readonly static FacilitySchedule FacilitySchedule3 = new()
    {
        FacilityId = Facility1.Id,
        DayOfWeek = DateTimeOffset.Now.DayOfWeek,
        OpensAt = new TimeOnly(8, 0),
        ClosesAt = new TimeOnly(20, 0),
    };

    public readonly static Reservation Reservation1 = new()
    {
        CourtId = Court1.Id,
        UserId = UserPlayer.Id,
        Period = new TimeRange(DateTime, DateTime.AddHours(1)),
        Status = ReservationStatus.Pending,
    };

    public readonly static Reservation Reservation2 = new()
    {
        CourtId = Court1.Id,
        UserId = UserPlayer.Id,
        Period = new TimeRange(DateTime.AddHours(1), DateTime.AddHours(2)),
        Status = ReservationStatus.Pending,
    };

    public readonly static Reservation Reservation3 = new()
    {
        CourtId = Court2.Id,
        UserId = UserPlayer.Id,
        Period = new TimeRange(DateTime.AddHours(2), DateTime.AddHours(3)),
        Status = ReservationStatus.Pending,
    };
}
