using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Handlers;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Reservations;

public class TestReservationsCreate(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    private readonly CreateReservationDTO _createReservationTest = new()
    {
        CourtId = InMemoryDataSource.Court1.Id,
        UserId = InMemoryDataSource.UserPlayer.Id,
        Period = new TimeRange(
            InMemoryDataSource.DateTime,
            InMemoryDataSource.DateTime.AddHours(1)
        ),
        Status = ReservationStatus.Pending,
    };

    [Fact]
    public async Task Should_CreateReservation()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner);
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RolePlayer);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserPlayer);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.Court1);
        await _fixtures.CreateEntityInMemory<FacilitySchedule>(
            InMemoryDataSource.FacilitySchedule1
        );

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<Reservation> result = await reservationHandler.Create(_createReservationTest);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Reservation>(result.Value);
        Assert.Equal(_createReservationTest.CourtId, result.Value.CourtId);
        Assert.Equal(_createReservationTest.UserId, result.Value.UserId);
        Assert.Equal(_createReservationTest.Period, result.Value.Period);
        Assert.Equal(_createReservationTest.Status, result.Value.Status);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_RejectedValidation()
    {
        // Arrange
        CreateReservationDTO _createReservationTestInvalidDate = new()
        {
            CourtId = InMemoryDataSource.Court1.Id,
            UserId = InMemoryDataSource.UserPlayer.Id,
            Period = new TimeRange(DateTime.Now, DateTime.Now.AddHours(-2)),
        };
        CreateReservationDTO _createReservationTestInvalidPastDate = new()
        {
            CourtId = InMemoryDataSource.Court1.Id,
            UserId = InMemoryDataSource.UserPlayer.Id,
            Period = new TimeRange(DateTime.Now.AddHours(-2), DateTime.Now),
        };
        CreateReservationDTO _createReservationTestInvalidDayPeriod = new()
        {
            CourtId = InMemoryDataSource.Court1.Id,
            UserId = InMemoryDataSource.UserPlayer.Id,
            Period = new TimeRange(DateTime.Now, DateTime.Now.AddDays(1)),
        };
        CreateReservationDTO _createReservationTestInvalidDuration = new()
        {
            CourtId = InMemoryDataSource.Court1.Id,
            UserId = InMemoryDataSource.UserPlayer.Id,
            Period = new TimeRange(DateTime.Now, DateTime.Now.AddHours(2)),
        };
        CreateReservationDTO _createReservationTestInvalidStatus = new()
        {
            CourtId = InMemoryDataSource.Court1.Id,
            UserId = InMemoryDataSource.UserPlayer.Id,
            Period = new TimeRange(
                InMemoryDataSource.DateTime,
                InMemoryDataSource.DateTime.AddHours(1)
            ),
            Status = (ReservationStatus)999,
        };

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<Reservation> resultDate = await reservationHandler.Create(
            _createReservationTestInvalidDate
        );
        Result<Reservation> resultPastDate = await reservationHandler.Create(
            _createReservationTestInvalidPastDate
        );
        Result<Reservation> resultDayPeriod = await reservationHandler.Create(
            _createReservationTestInvalidDayPeriod
        );
        Result<Reservation> resultDuration = await reservationHandler.Create(
            _createReservationTestInvalidDuration
        );
        Result<Reservation> resultStatus = await reservationHandler.Create(
            _createReservationTestInvalidStatus
        );

        // Assert
        Assert.Null(resultDate.Value);
        Assert.Equal(ReservationsErrorResults.CreationInvalidDates, resultDate.Error);
        Assert.Null(resultPastDate.Value);
        Assert.Equal(ReservationsErrorResults.CreationStartAndEndInThePast, resultPastDate.Error);
        Assert.Null(resultDayPeriod.Value);
        Assert.Equal(
            ReservationsErrorResults.CreationStartAndEndNotInTheSameDay,
            resultDayPeriod.Error
        );
        Assert.Null(resultDuration.Value);
        Assert.Equal(
            ReservationsErrorResults.CreationDurationTimeDifferentThanConfiguration,
            resultDuration.Error
        );
        Assert.Null(resultStatus.Value);
        Assert.Equal(ReservationsErrorResults.CreationInvalidStatus, resultStatus.Error);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_NoCourtFound()
    {
        // Arrange
        await _fixtures.RemoveAllDataFromMemory<Facility>();
        await _fixtures.RemoveAllDataFromMemory<FacilitySchedule>();
        await _fixtures.RemoveAllDataFromMemory<Court>();

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<Reservation> result = await reservationHandler.Create(_createReservationTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.CreationUnknownCourt, result.Error);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_NoUserFound()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.Court1);
        await _fixtures.RemoveDataFromMemory<User>(InMemoryDataSource.UserPlayer.Id);

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<Reservation> result = await reservationHandler.Create(_createReservationTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.CreationUnknownUser, result.Error);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_NoScheduleFound()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner);
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RolePlayer);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserPlayer);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.Court1);
        await _fixtures.RemoveAllDataFromMemory<FacilitySchedule>();

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<Reservation> result = await reservationHandler.Create(_createReservationTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.CreationOutsideFacilitySchedule, result.Error);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_OverlappingReservation()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner);
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RolePlayer);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserPlayer);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.Court1);
        await _fixtures.CreateEntityInMemory<FacilitySchedule>(
            InMemoryDataSource.FacilitySchedule1
        );
        await _fixtures.CreateEntityInMemory<Reservation>(InMemoryDataSource.Reservation1);

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        CreateReservationDTO _createReservationTestOverlapping = new()
        {
            CourtId = InMemoryDataSource.Court1.Id,
            UserId = InMemoryDataSource.UserPlayer.Id,
            Period = new TimeRange(
                InMemoryDataSource.DateTime.AddMinutes(30),
                InMemoryDataSource.DateTime.AddHours(1).AddMinutes(30)
            ),
        };

        // Act
        Result<Reservation> result = await reservationHandler.Create(
            _createReservationTestOverlapping
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.CreationOverlappingReservation, result.Error);
    }
}
