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
    public async Task Should_UpdateFacilitySchedule()
    {
        // Arrange
        using var context = fixtures.CreateContext();
        Reservation toBeUpdated = await DbOperations.CreateRecord(TestDataSource.ToBeUpdatedReservation, context);

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        TestUpdateStatusFromReservationDTO reservationDTO = _updateReservationTest with
        {
            Id = toBeUpdated.Id,
            UserId = toBeUpdated.UserId,
        };

        // Act
        Result<Reservation> result = await reservationHandler.UpdateReservationStatus(
            _updateReservationTest
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Reservation>(result.Value);
        Assert.Equal(_updateReservationTest.Status, result.Value.Status);
        Assert.Equal(_updateReservationTest.UserId, result.Value.UserId);

        await DbOperations.RemoveRecord<Reservation>(result.Value.Id, context);
    }

    [Fact]
    public async Task Should_Not_HaveValidStatus()
    {
        // Arrange
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
