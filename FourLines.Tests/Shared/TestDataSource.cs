using FourLines.Application.DTOs.Reservations;
using FourLines.Domain.Constants;
using FourLines.Domain.Models;

namespace FourLines.Tests.Shared;

public class TestDataSource
{
    public static readonly Guid RoleOwnerId = new("17b044a4-3c53-4fc9-86b1-16f314877de0");
    public static readonly Guid RolePlayerId = new("76199b24-62ef-49f1-816e-decfa45c2900");
    public static readonly Guid UserOwnerId = new("e7021ffc-ab35-4475-b640-a6b23742a132");
    public static readonly Guid UserPlayerId = new("1a8997ec-171f-4af2-82b1-49a914b2a526");
    public static readonly Guid DefaultFacilityId = new("40b43ff5-2ad0-4a82-b7df-574f2f7ca716");
    public static readonly Guid DefaultSportId = new("ad4eb568-bbe0-43fe-a6ed-3e1bb1e05340");
    public static readonly Guid DefaultCourtId = new("1c07cc5e-036e-4d23-a3ed-ac8d64b2968c");
    public static readonly Guid DefaultFacilityScheduleId = new("26a011e8-25a8-4b91-8471-0c4f1854eee4");

    public static readonly DateTimeOffset DateTimeNow = DateTimeOffset.UtcNow;
    public static readonly DateTimeOffset SettedDateTime = 
        new(DateTimeOffset.UtcNow.Year + 1, 2, 1, 22, 0, 0, TimeSpan.Zero);

    public static readonly Role RoleOwner = new()
    {
        Name = RoleConstants.FacilityOwner
    };
    public static readonly Role RolePlayer = new()
    {
        Name = RoleConstants.Player
    };

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

    public static readonly User UserPlayer2 = new()
    {
        RoleId = RolePlayer.Id,
        Name = "Jane Smith",
        Email = "jane.smith2@example.com",
        PasswordHash = "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1970, 1, 1),
        Phone = "55 54 9 9999-9999",
        RegistrationNumber = "383.975.212-89",
    };
    public static readonly User UserPlayer3 = new()
    {
        RoleId = RolePlayer.Id,
        Name = "Jane Smith",
        Email = "jane.smith3@example.com",
        PasswordHash = "AQAAAAIAAYagAAAAEMIamrvIuvlWmAnvN+crLN6139ExUi8CuZC2s6J4W/h7DNKU+Z8syKwX08xHWmZp+g==",
        Birthday = new DateOnly(1970, 1, 1),
        Phone = "55 54 9 9999-9999",
        RegistrationNumber = "383.975.213-89",
    };

    public static readonly Facility DefaultFacility = new()
    {
        Name = "Default Facility 1",
        Address = "123 Test St",
        City = "Test City",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "1234567890",
        OwnerId = UserOwner.Id,
    };

    public static readonly Facility DefaultFacility2 = new()
    {
        Name = "Default Facility 2",
        Address = "456 Test Ave",
        City = "Test City 2",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "0987654458",
        OwnerId = UserOwner.Id,
    };

    public static readonly Facility DefaultNoSchedulesFacility = new()
    {
        Name = "No schedules facility",
        Address = "789 Test Blvd",
        City = "Test City",
        State = "TS",
        ZipCode = "12346",
        RegistrationNumber = "0994654389",
        OwnerId = UserOwner.Id,
    };

    public static readonly Facility DefaultNoSchedulesFacility2 = new()
    {
        Name = "No schedules facility",
        Address = "789 Test Blvd",
        City = "Test City",
        State = "TS",
        ZipCode = "12346",
        RegistrationNumber = "0994874389",
        OwnerId = UserOwner.Id,
    };

    public static readonly Facility ToBeUpdatedFacility = new()
    {
        Name = "To be updated",
        Address = "456 Test Ave",
        City = "Test City 2",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "0987654987",
        OwnerId = UserOwner.Id,
    };

    public static readonly Facility ToBeDeletedFacility = new()
    {
        Name = "To be deleted",
        Address = "456 Test Ave",
        City = "Test City 2",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "0986324987",
        OwnerId = UserOwner.Id,
    };

    public static readonly Facility DummyFacility = new()
    {
        Name = "Dummy Facility",
        Address = "789 Test Blvd",
        City = "Test City 3",
        State = "TS",
        ZipCode = "12346",
        RegistrationNumber = "0987654345",
        OwnerId = UserOwner.Id,
    };

    public static readonly Sport DefaultSport = new()
    {
        Name = "Test Sport",
        Indoor = true,
        StartingPlayersCount = 5,
        MaxPlayersCount = 10,
        HasFixedGoalKeeper = true,
    };

    public static readonly Sport SportWithoutGoalkeeper = new()
    {
        Name = "Test Sport 2",
        Indoor = true,
        StartingPlayersCount = 5,
        MaxPlayersCount = 10,
        HasFixedGoalKeeper = false,
    };

    public static readonly Court DefaultCourt = new()
    {
        FacilityId = DefaultFacility.Id,
        SportId = DefaultSport.Id,
        Name = "Test Court",
        IsActive = true,
    };
    public static readonly Court CourtWithNoSchedule = new()
    {
        FacilityId = DefaultNoSchedulesFacility.Id,
        SportId = DefaultSport.Id,
        Name = "Test Court 2",
        IsActive = true,
    };

    public static readonly Court CourtWithNoSchedule2 = new()
    {
        FacilityId = DefaultNoSchedulesFacility2.Id,
        SportId = DefaultSport.Id,
        Name = "Test Court 2",
        IsActive = true,
    };

    public static readonly Court ToBeUpdatedCourt = new()
    {
        FacilityId = DefaultFacility.Id,
        SportId = DefaultSport.Id,
        Name = "To be updated",
        IsActive = true,
    };

    public static readonly Court ToBeDeletedCourt = new()
    {
        FacilityId = DefaultFacility.Id,
        SportId = DefaultSport.Id,
        Name = "To be deleted",
        IsActive = true,
    };

    public static readonly Court Court2 = new()
    {
        FacilityId = DefaultFacility.Id,
        SportId = DefaultSport.Id,
        Name = "Test Court 2",
        IsActive = true,
    };
    public static readonly Court Court3 = new()
    {
        FacilityId = DummyFacility.Id,
        SportId = SportWithoutGoalkeeper.Id,
        Name = "Test Court 3",
        IsActive = true,
    };
    public static readonly Court CourtWithSportWithoutGoalkeeper = new()
    {
        FacilityId = DefaultFacility.Id,
        SportId = SportWithoutGoalkeeper.Id,
        Name = "Test Court 4",
        IsActive = true,
    };

    public static readonly FacilitySchedule DefaultFacilityScheduleSunday = new()
    {
        FacilityId = DefaultFacility.Id,
        DayOfWeek = DayOfWeek.Sunday,
        OpensAt = new TimeOnly(0, 0),
        ClosesAt = new TimeOnly(23, 59),
    };

    public static readonly FacilitySchedule DefaultFacilityScheduleMonday = new()
    {
        FacilityId = DefaultFacility.Id,
        DayOfWeek = DayOfWeek.Monday,
        OpensAt = new TimeOnly(0, 0),
        ClosesAt = new TimeOnly(23, 59),
    };

    public static readonly FacilitySchedule DefaultFacilityScheduleTuesday = new()
    {
        FacilityId = DefaultFacility.Id,
        DayOfWeek = DayOfWeek.Tuesday,
        OpensAt = new TimeOnly(0, 0),
        ClosesAt = new TimeOnly(23, 59),
    };

    public static readonly FacilitySchedule DefaultFacilityScheduleWednesday = new()
    {
        FacilityId = DefaultFacility.Id,
        DayOfWeek = DayOfWeek.Wednesday,
        OpensAt = new TimeOnly(0, 0),
        ClosesAt = new TimeOnly(23, 59),
    };

    public static readonly FacilitySchedule DefaultFacilityScheduleThursday = new()
    {
        FacilityId = DefaultFacility.Id,
        DayOfWeek = DayOfWeek.Thursday,
        OpensAt = new TimeOnly(0, 0),
        ClosesAt = new TimeOnly(23, 59),
    };

    public static readonly FacilitySchedule DefaultFacilityScheduleFriday = new()
    {
        FacilityId = DefaultFacility.Id,
        DayOfWeek = DayOfWeek.Friday,
        OpensAt = new TimeOnly(0, 0),
        ClosesAt = new TimeOnly(23, 59),
    };

    public static readonly FacilitySchedule DefaultFacilityScheduleSaturday = new()
    {
        FacilityId = DefaultFacility.Id,
        DayOfWeek = DayOfWeek.Saturday,
        OpensAt = new TimeOnly(0, 0),
        ClosesAt = new TimeOnly(23, 59),
    };

    public static readonly FacilitySchedule ToBeDeletedFacilitySchedule = new()
    {
        FacilityId = DefaultNoSchedulesFacility.Id,
        DayOfWeek = DayOfWeek.Thursday,
        OpensAt = new TimeOnly(8, 0),
        ClosesAt = new TimeOnly(20, 0),
    };

    public static readonly FacilitySchedule ToBeUpdatedFacilitySchedule = new()
    {
        FacilityId = DefaultNoSchedulesFacility.Id,
        DayOfWeek = DayOfWeek.Friday,
        OpensAt = new TimeOnly(0, 0),
        ClosesAt = new TimeOnly(23, 59),
    };

    public static readonly Reservation DefaultReservation = new()
    {
        CourtId = DefaultCourt.Id,
        UserId = UserPlayer.Id,
        Period = new TimeRange(DateTimeNow, DateTimeNow.AddHours(1)),
        Status = ReservationStatus.Pending,
    };

    public static readonly Reservation ToBeDeletedReservation = new()
    {
        CourtId = DefaultCourt.Id,
        UserId = UserPlayer.Id,
        Period = new TimeRange(DateTimeNow.AddHours(1), DateTimeNow.AddHours(2)),
        Status = ReservationStatus.Pending,
    };

    public static readonly Reservation ToBeUpdatedReservation = new()
    {
        CourtId = DefaultCourt.Id,
        UserId = UserPlayer.Id,
        Period = new TimeRange(DateTimeNow.AddHours(2), DateTimeNow.AddHours(3)),
        Status = ReservationStatus.Pending,
    };

    public static readonly CreateReservationDTO CreateGoalKeeperReservationTest = new()
    {
        CourtId = DefaultCourt.Id,
        UserId = UserPlayer.Id,
        Period = new TimeRange(
            SettedDateTime.AddHours(14),
            SettedDateTime.AddHours(15)
        ),
        Status = ReservationStatus.Pending,
    };

    public static readonly CreateReservationDTO CreateNoGoalKeeperReservationTest = new()
    {
        CourtId = CourtWithSportWithoutGoalkeeper.Id,
        UserId = UserPlayer.Id,
        Period = new TimeRange(
            SettedDateTime.AddHours(15),
            SettedDateTime.AddHours(16)
        ),
        Status = ReservationStatus.Pending,
    };
}
