using FourLines.Application.DTOs.Facilities;
using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Handlers;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Reservations;

public class TestReservationsUpdate(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    [Fact]
    public async Task Should_UpdateFacilitySchedule()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.rolePlayer);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userPlayer);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.sport);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.court1);
        await _fixtures.CreateEntityInMemory<FacilitySchedule>(
            InMemoryDataSource.facilitySchedule1
        );
        await _fixtures.CreateEntityInMemory<Reservation>(InMemoryDataSource.reservation1);

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        UpdateStatusFromReservationDTO updateReservationTest = new()
        {
            Id = InMemoryDataSource.reservation1.Id,
            UserId = InMemoryDataSource.userPlayer.Id,
            Status = ReservationStatus.Confirmed,
        };

        // Act
        Result<Reservation> result = await reservationHandler.UpdateStatusFromReservation(
            updateReservationTest
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Reservation>(result.Value);
        Assert.Equal(updateReservationTest.Status, result.Value.Status);
        Assert.Equal(updateReservationTest.UserId, result.Value.UserId);
    }

    [Fact]
    public async Task Should_Not_HaveValidStatus()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.rolePlayer);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userPlayer);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.sport);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.court1);
        await _fixtures.CreateEntityInMemory<FacilitySchedule>(
            InMemoryDataSource.facilitySchedule1
        );
        await _fixtures.CreateEntityInMemory<Reservation>(InMemoryDataSource.reservation1);

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        UpdateStatusFromReservationDTO updateReservationTest = new()
        {
            Id = InMemoryDataSource.reservation1.Id,
            UserId = InMemoryDataSource.userPlayer.Id,
            Status = (ReservationStatus) 999,
        };

        // Act
        Result<Reservation> result = await reservationHandler.UpdateStatusFromReservation(
            updateReservationTest
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.CreationInvalidStatus, result.Error);
    }

    [Fact]
    public async Task Should_Not_AffectAnyRowFacilitySchedule()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.rolePlayer);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userPlayer);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.sport);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.court1);
        await _fixtures.CreateEntityInMemory<FacilitySchedule>(
            InMemoryDataSource.facilitySchedule1
        );
        await _fixtures.CreateEntityInMemory<Reservation>(InMemoryDataSource.reservation1);

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        UpdateStatusFromReservationDTO updateReservationTest = new()
        {
            Id = Guid.NewGuid(),
            UserId = InMemoryDataSource.userPlayer.Id,
            Status = ReservationStatus.Confirmed,
        };

        // Act
        Result<Reservation> result = await reservationHandler.UpdateStatusFromReservation(
            updateReservationTest
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.UpdateReservationDoesNotExist, result.Error);
    }
}
