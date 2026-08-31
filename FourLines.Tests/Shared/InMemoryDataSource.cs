using FourLines.Domain.Constants;
using FourLines.Domain.Models;

namespace FourLines.Tests.Shared;

public class InMemoryDataSource
{
    public static readonly Guid RoleOwnerId = new("17b044a4-3c53-4fc9-86b1-16f314877de0");
    public static readonly Guid RolePlayerId = new("76199b24-62ef-49f1-816e-decfa45c2900");
    public static readonly Guid UserOwnerId = new("e7021ffc-ab35-4475-b640-a6b23742a132");
    public static readonly Guid UserPlayerId = new("1a8997ec-171f-4af2-82b1-49a914b2a526");
    public static readonly Guid FacilityId = new("40b43ff5-2ad0-4a82-b7df-574f2f7ca716");
    public static readonly Guid TestSportId = new("ad4eb568-bbe0-43fe-a6ed-3e1bb1e05340");
    public static readonly Guid CourtId = new("1c07cc5e-036e-4d23-a3ed-ac8d64b2968c");
    public static readonly Guid FacilityScheduleId = new("26a011e8-25a8-4b91-8471-0c4f1854eee4");

    public static readonly DateTimeOffset DateTime = DateTimeOffset.Now;
    public static readonly DateTimeOffset SettedDateTime = new(2026, 8, 31, 10, 0, 0, TimeSpan.Zero);
    public static readonly Role RoleOwner = new() { Name = RoleConstants.FacilityOwner };
    public static readonly Role RolePlayer = new() { Name = RoleConstants.Player };

    public static readonly User UserOwner = new()
    {
        RoleId = RoleOwner.Id,
        Name = "John Doe",
        Email = "john.doe@example.com",
        PasswordHash = "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1970, 1, 1),
        Phone = "55 54 9 9999-9999",
        RegistrationNumber = "383.975.210-89",
    };

    public static readonly User UserPlayer = new()
    {
        RoleId = RolePlayer.Id,
        Name = "Jane Smith",
        Email = "jane.smith@example.com",
        PasswordHash = "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1970, 1, 1),
        Phone = "55 54 9 9999-9999",
        RegistrationNumber = "383.975.211-89",
    };

    public static readonly Facility Facility1 = new()
    {
        Name = "Test Facility 1",
        Address = "123 Test St",
        City = "Test City",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "1234567890",
        OwnerId = UserOwner.Id,
    };

    public static readonly Facility Facility2 = new()
    {
        Name = "Test Facility 2",
        Address = "456 Test Ave",
        City = "Test City 2",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "0987654321",
        OwnerId = UserOwner.Id,
    };

    public static readonly Sport TestSport = new()
    {
        Name = "Test Sport",
        Indoor = true,
        StartingPlayersCount = 5,
        MaxPlayersCount = 10,
    };

    public static readonly Court Court1 = new()
    {
        FacilityId = Facility1.Id,
        SportId = TestSport.Id,
        Name = "Test Court",
        IsActive = true,
    };

    public static readonly Court Court2 = new()
    {
        FacilityId = Facility1.Id,
        SportId = TestSport.Id,
        Name = "Test Court 2",
        IsActive = true,
    };

    public static readonly FacilitySchedule FacilitySchedule1 = new()
    {
        FacilityId = Facility1.Id,
        DayOfWeek = DayOfWeek.Tuesday,
        OpensAt = new TimeOnly(8, 0),
        ClosesAt = new TimeOnly(20, 0),
    };

    public static readonly FacilitySchedule FacilitySchedule2 = new()
    {
        FacilityId = Facility1.Id,
        DayOfWeek = DayOfWeek.Thursday,
        OpensAt = new TimeOnly(8, 0),
        ClosesAt = new TimeOnly(20, 0),
    };

    public static readonly FacilitySchedule FacilitySchedule3 = new()
    {
        FacilityId = Facility1.Id,
        DayOfWeek = DateTimeOffset.Now.DayOfWeek,
        OpensAt = new TimeOnly(0, 0),
        ClosesAt = new TimeOnly(23, 59),
    };

    public static readonly Reservation Reservation1 = new()
    {
        CourtId = Court1.Id,
        UserId = UserPlayer.Id,
        Period = new TimeRange(DateTime, DateTime.AddHours(1)),
        Status = ReservationStatus.Pending,
    };

    public static readonly Reservation Reservation2 = new()
    {
        CourtId = Court1.Id,
        UserId = UserPlayer.Id,
        Period = new TimeRange(DateTime.AddHours(1), DateTime.AddHours(2)),
        Status = ReservationStatus.Pending,
    };

    public static readonly Reservation Reservation3 = new()
    {
        CourtId = Court2.Id,
        UserId = UserPlayer.Id,
        Period = new TimeRange(DateTime.AddHours(2), DateTime.AddHours(3)),
        Status = ReservationStatus.Pending,
    };
}
