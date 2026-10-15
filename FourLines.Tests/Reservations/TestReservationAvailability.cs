using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;
using FourLines.Tests.Shared.Seed;

namespace FourLines.Tests.Reservations;

[Collection(FourLinesCollection.Name)]
public class TestReservationAvailability(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_CheckAvailability()
    {
        // Arrange
        Guid courtId = CourtSeed.Default.Id;
        DateTime selectedDate = DateTime.Now;
        CancellationToken cancellationToken = default;

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        // Act
        Result<CourtAvailability> result = await reservationHandler.GetAvailableTimesToPlayInCourt(
            courtId,
            selectedDate,
            cancellationToken
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.True(result.IsSuccess);
        Assert.Equal(23, result.Value.TimeSlots.Count());
    }
}