using DotNet.Testcontainers.Configurations;
using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Infrastructure.Contexts;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Reservations;

[Collection(FourLinesCollection.Name)]
public class TestReservationsRead(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_GetAllReservationsFromUser()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        FourLinesContext context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();
        Guid userId = TestDataSource.UserPlayer.Id;

        // Act
        Result<IEnumerable<Reservation>> result =
            await reservationHandler.GetAllReservationsFromUser(userId);

        // Assert
        int reservationsFromUser = context.Reservations.Where(r => r.UserId == userId).Count();

        Assert.NotEmpty(result.Value);
        Assert.Equal(reservationsFromUser, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetAllReservationsFromUser()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<IEnumerable<Reservation>> result =
            await reservationHandler.GetAllReservationsFromUser(Guid.NewGuid());

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.GetAllNoReservationsForUser, result.Error);
    }

    [Fact]
    public async Task Should_GetAllReservationsFromCourt()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        FourLinesContext context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();
        Guid courtId = TestDataSource.DefaultCourt.Id;

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<IEnumerable<Reservation>> result =
            await reservationHandler.GetAllReservationsFromCourt(TestDataSource.DefaultCourt.Id);

        // Assert
        int reservationsFromCourt = context.Reservations.Where(r => r.CourtId == courtId).Count();

        Assert.NotEmpty(result.Value);
        Assert.Equal(reservationsFromCourt, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetAllReservationsFromCourt()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<IEnumerable<Reservation>> result =
            await reservationHandler.GetAllReservationsFromCourt(Guid.NewGuid());

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.GetAllNoReservationsForCourt, result.Error);
    }

    [Fact]
    public async Task Should_GetOneReservationFromUser()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<Reservation> result = await reservationHandler.GetOneReservationFromUser(
            TestDataSource.DefaultReservation.UserId,
            TestDataSource.DefaultReservation.Id
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(result.Value.CourtId, result.Value.CourtId);
        Assert.Equal(result.Value.UserId, result.Value.UserId);
        Assert.Equal(result.Value.Status, result.Value.Status);
    }

    [Fact]
    public async Task Should_Not_GetOneReservationFromUser()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

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
