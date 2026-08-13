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
        await _fixtures.CreateEntityInMemory<Reservation>(InMemoryDataSource.Reservation2);

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<IEnumerable<Reservation>> result =
            await reservationHandler.GetAllReservationsFromUser(InMemoryDataSource.UserPlayer.Id);

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
            await reservationHandler.GetAllReservationsFromUser(InMemoryDataSource.UserPlayer.Id);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.GetAllNoReservationsForUser, result.Error);
    }

    [Fact]
    public async Task Should_GetAllReservationsFromCourt()
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
        await _fixtures.CreateEntityInMemory<Reservation>(InMemoryDataSource.Reservation2);

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<IEnumerable<Reservation>> result =
            await reservationHandler.GetAllReservationsFromCourt(InMemoryDataSource.Court1.Id);

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
            await reservationHandler.GetAllReservationsFromCourt(InMemoryDataSource.Court1.Id);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.GetAllNoReservationsForCourt, result.Error);
    }

    [Fact]
    public async Task Should_GetOneReservationFromUser()
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

        // Act
        Result<Reservation> result = await reservationHandler.GetOneReservationFromUser(
            InMemoryDataSource.UserPlayer.Id,
            InMemoryDataSource.Reservation1.Id
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(InMemoryDataSource.Reservation1.CourtId, result.Value.CourtId);
        Assert.Equal(InMemoryDataSource.Reservation1.UserId, result.Value.UserId);
        Assert.Equal(InMemoryDataSource.Reservation1.Period, result.Value.Period);
        Assert.Equal(InMemoryDataSource.Reservation1.Status, result.Value.Status);
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
