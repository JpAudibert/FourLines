using FourLines.Application.DTOs.Reservations.Interfaces;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Reservations;

public record TestUpdateStatusFromReservationDTO : IUpdateStatusFromReservationDTO
{
    public Guid Id { get; init; }
    public ReservationStatus Status { get; init; }
    public Guid UserId { get; init; }
}

[Collection(FourLinesCollection.Name)]
public class TestReservationsUpdate(FourLinesFixture fixtures)
{
    private static readonly TestUpdateStatusFromReservationDTO _updateReservationTest = new()
    {
        Id = TestDataSource.ToBeUpdatedReservation.Id,
        UserId = TestDataSource.UserPlayer.Id,
        Status = ReservationStatus.Confirmed,
    };

    [Fact]
    public async Task Should_UpdateReservation()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();
        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        TestUpdateStatusFromReservationDTO reservationDTO = _updateReservationTest with
        {
            Id = TestDataSource.ToBeUpdatedReservation.Id,
            UserId = TestDataSource.ToBeUpdatedReservation.UserId,
        };

        // Act
        Result<Reservation> result = await reservationHandler.UpdateReservationStatus(
            reservationDTO
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Reservation>(result.Value);
        Assert.Equal(reservationDTO.Status, result.Value.Status);
        Assert.Equal(reservationDTO.UserId, result.Value.UserId);
    }

    [Fact]
    public async Task Should_Not_HaveValidStatus()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();
        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        TestUpdateStatusFromReservationDTO reservationWithInvalidStatus = _updateReservationTest with
        {
            Status = (ReservationStatus)999,
        };

        // Act
        Result<Reservation> result = await reservationHandler.UpdateReservationStatus(
            reservationWithInvalidStatus
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.CreationInvalidStatus, result.Error);
    }

    [Fact]
    public async Task Should_Not_AffectAnyRowFacilitySchedule()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();
        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        TestUpdateStatusFromReservationDTO reservationWithInvalidId = _updateReservationTest with
        {
            Id = Guid.NewGuid(),
        };

        // Act
        Result<Reservation> result = await reservationHandler.UpdateReservationStatus(
            reservationWithInvalidId
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.UpdateReservationDoesNotExist, result.Error);
    }
}
