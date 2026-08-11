using FourLines.Application.Handlers;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Reservations;

public class TestReservationsRead(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    [Fact]
    public async Task Should_GetAllReservationsFromUser()
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
        await _fixtures.CreateEntityInMemory<Reservation>(InMemoryDataSource.reservation2);

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<IEnumerable<Reservation>> result =
            await reservationHandler.GetAllReservationsFromUser(InMemoryDataSource.userPlayer.Id);

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetAllReservationsFromUser()
    {
        // Arrange
        await _fixtures.RemoveAllDataFromMemory<Reservation>();

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<IEnumerable<Reservation>> result =
            await reservationHandler.GetAllReservationsFromUser(InMemoryDataSource.userPlayer.Id);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.GetAllNoReservationsForUser, result.Error);
    }

    [Fact]
    public async Task Should_GetAllReservationsFromCourt()
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
        await _fixtures.CreateEntityInMemory<Reservation>(InMemoryDataSource.reservation2);

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<IEnumerable<Reservation>> result =
            await reservationHandler.GetAllReservationsFromCourt(InMemoryDataSource.court1.Id);

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetAllReservationsFromCourt()
    {
        // Arrange
        await _fixtures.RemoveAllDataFromMemory<Reservation>();

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<IEnumerable<Reservation>> result =
            await reservationHandler.GetAllReservationsFromCourt(InMemoryDataSource.court1.Id);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.GetAllNoReservationsForCourt, result.Error);
    }

    [Fact]
    public async Task Should_GetOneReservationFromUser()
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

        // Act
        Result<Reservation> result = await reservationHandler.GetOneReservationFromUser(
            InMemoryDataSource.userPlayer.Id,
            InMemoryDataSource.reservation1.Id
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(InMemoryDataSource.reservation1.CourtId, result.Value.CourtId);
        Assert.Equal(InMemoryDataSource.reservation1.UserId, result.Value.UserId);
        Assert.Equal(InMemoryDataSource.reservation1.Period, result.Value.Period);
        Assert.Equal(InMemoryDataSource.reservation1.Status, result.Value.Status);
    }

    [Fact]
    public async Task Should_Not_GetOneReservationFromUser()
    {
        // Arrange
        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<Reservation> result = await reservationHandler.GetOneReservationFromUser(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.GetOneReservationDoesNotExist, result.Error);
    }
}
