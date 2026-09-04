using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Reservations;

public class TestReservationsRead(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    [Fact]
    public async Task Should_GetAllReservationsFromUser()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RolePlayer, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserPlayer, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport, context);
            await DbOperations.CreateEntityInMemory<Court>(InMemoryDataSource.Court1, context);
            await DbOperations.CreateEntityInMemory<FacilitySchedule>(InMemoryDataSource.FacilitySchedule1, context);
            await DbOperations.CreateEntityInMemory<Reservation>(InMemoryDataSource.Reservation1, context);
            await DbOperations.CreateEntityInMemory<Reservation>(InMemoryDataSource.Reservation2, context);
        }

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

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
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.RemoveAllDataFromMemory<Reservation>(context);
        }

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

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
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RolePlayer, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserPlayer, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport, context);
            await DbOperations.CreateEntityInMemory<Court>(InMemoryDataSource.Court1, context);
            await DbOperations.CreateEntityInMemory<FacilitySchedule>(InMemoryDataSource.FacilitySchedule1, context);
            await DbOperations.CreateEntityInMemory<Reservation>(InMemoryDataSource.Reservation1, context);
            await DbOperations.CreateEntityInMemory<Reservation>(InMemoryDataSource.Reservation2, context);
        }

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

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
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.RemoveAllDataFromMemory<Reservation>(context);
        }

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

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
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RolePlayer, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserPlayer, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport, context);
            await DbOperations.CreateEntityInMemory<Court>(InMemoryDataSource.Court1, context);
            await DbOperations.CreateEntityInMemory<FacilitySchedule>(InMemoryDataSource.FacilitySchedule1, context);
            await DbOperations.CreateEntityInMemory<Reservation>(InMemoryDataSource.Reservation1, context);
        }

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

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
        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

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
