using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Reservations;

[Collection(FourLinesCollection.Name)]
public class TestReservationsRead(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_GetAllReservationsFromUser()
    {
        // Arrange
        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<IEnumerable<Reservation>> result =
            await reservationHandler.GetAllReservationsFromUser(TestDataSource.UserPlayer.Id);

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(4, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetAllReservationsFromUser()
    {
        // Arrange
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
        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<IEnumerable<Reservation>> result =
            await reservationHandler.GetAllReservationsFromCourt(TestDataSource.DefaultCourt.Id);

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(4, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetAllReservationsFromCourt()
    {
        // Arrange
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
        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Create a test reservation instead of relying on seeded data
        var createDto = new TestCreateReservationDTO
        {
            CourtId = TestDataSource.DefaultCourt.Id,
            UserId = TestDataSource.UserPlayer2.Id,
            Period = new TimeRange(
                TestDataSource.DateTimeNow.AddHours(3),
                TestDataSource.DateTimeNow.AddHours(4)
            ),
            Status = ReservationStatus.Pending
        };

        var createResult = await reservationHandler.Create(createDto);
        Assert.NotNull(createResult.Value);
        var createdReservationId = createResult.Value.Reservation.Id;

        // Act
        Result<Reservation> result = await reservationHandler.GetOneReservationFromUser(
            TestDataSource.UserPlayer2.Id,
            createdReservationId
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(createDto.CourtId, result.Value.CourtId);
        Assert.Equal(createDto.UserId, result.Value.UserId);
        Assert.Equal(createDto.Status, result.Value.Status);
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
