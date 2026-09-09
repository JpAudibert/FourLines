using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Reservations;

[Collection(ReservationsCollection.Name)]
public class TestReservationsDelete(ReservationsFixture fixtures)
{
    [Fact]
    public async Task Should_DeleteReservation()
    {
        // Arrange
        await using var context = fixtures.CreateContext();
        Reservation testReservation = await DbOperations.CreateRecord<Reservation>(TestDataSource.Reservation2, context);

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<bool> result = await reservationHandler.Delete(new DeleteReservationDTO
        {
            UserId = testReservation.UserId,
            ReservationId = testReservation.Id
        });

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteReservation()
    {
        // Arrange
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
