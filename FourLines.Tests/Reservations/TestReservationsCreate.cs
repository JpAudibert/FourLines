using FourLines.Application.DTOs.FacilitySchedules;
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
        CourtId = InMemoryDataSource.court1.Id,
        UserId = InMemoryDataSource.userPlayer.Id,
        Period = new TimeRange(
            new DateTime(2024, 6, 10, 10, 0, 0),
            new DateTime(2024, 6, 10, 11, 0, 0)
        ),
        Status = ReservationStatus.Pending,
    };

    [Fact]
    public async Task Should_CreateReservation()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.sport);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.court1);
        await _fixtures.CreateEntityInMemory<FacilitySchedule>(
            InMemoryDataSource.facilitySchedule1
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
        await _fixtures.RemoveAllDataFromMemory<Facility>();
        await _fixtures.RemoveAllDataFromMemory<FacilitySchedule>();

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<Reservation> result = await reservationHandler.Create(_createReservationTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitySchedulesErrorResults.CreateFacilitySchedules, result.Error);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_NoCourtFound()
    {
        // Arrange
        await _fixtures.RemoveAllDataFromMemory<Facility>();
        await _fixtures.RemoveAllDataFromMemory<FacilitySchedule>();

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<Reservation> result = await reservationHandler.Create(_createReservationTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitySchedulesErrorResults.CreateFacilitySchedules, result.Error);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_NoUserFound()
    {
        // Arrange
        await _fixtures.RemoveAllDataFromMemory<Facility>();
        await _fixtures.RemoveAllDataFromMemory<FacilitySchedule>();

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<Reservation> result = await reservationHandler.Create(_createReservationTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitySchedulesErrorResults.CreateFacilitySchedules, result.Error);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_NoScheduleFound()
    {
        // Arrange
        await _fixtures.RemoveAllDataFromMemory<Facility>();
        await _fixtures.RemoveAllDataFromMemory<FacilitySchedule>();

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<Reservation> result = await reservationHandler.Create(_createReservationTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitySchedulesErrorResults.CreateFacilitySchedules, result.Error);
    }

    [Fact]
    public async Task Should_Not_CreateReservation_OverlappingReservation()
    {
        // Arrange
        await _fixtures.RemoveAllDataFromMemory<Facility>();
        await _fixtures.RemoveAllDataFromMemory<FacilitySchedule>();

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<Reservation> result = await reservationHandler.Create(_createReservationTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitySchedulesErrorResults.CreateFacilitySchedules, result.Error);
    }
}
