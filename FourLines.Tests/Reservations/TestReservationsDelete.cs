using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Interfaces;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Reservations;

[Collection(FourLinesCollection.Name)]
public class TestReservationsDelete(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_DeleteReservation()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<bool> result = await reservationHandler.Delete(new DeleteReservationDTO
        {
            UserId = TestDataSource.ToBeDeletedReservation.UserId,
            ReservationId = TestDataSource.ToBeDeletedReservation.Id
        });

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteReservation()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<bool> result = await reservationHandler.Delete(new DeleteReservationDTO
        {
            UserId = Guid.NewGuid(),
            ReservationId = Guid.NewGuid()
        });

        // Assert
        Assert.False(result.Value);
    }
}
