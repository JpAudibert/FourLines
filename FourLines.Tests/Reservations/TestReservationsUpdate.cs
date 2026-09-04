using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Reservations;

public class TestReservationsUpdate(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    [Fact]
    public async Task Should_UpdateFacilitySchedule()
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
            await DbOperations.CreateEntityInMemory<FacilitySchedule>(
                InMemoryDataSource.FacilitySchedule1,
                context
            );
            await DbOperations.CreateEntityInMemory<Reservation>(InMemoryDataSource.Reservation1, context);
        }

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        UpdateStatusFromReservationDTO updateReservationTest = new()
        {
            Id = InMemoryDataSource.Reservation1.Id,
            UserId = InMemoryDataSource.UserPlayer.Id,
            Status = ReservationStatus.Confirmed,
        };

        // Act
        Result<Reservation> result = await reservationHandler.UpdateReservationStatus(
            updateReservationTest
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Reservation>(result.Value);
        Assert.Equal(updateReservationTest.Status, result.Value.Status);
        Assert.Equal(updateReservationTest.UserId, result.Value.UserId);
    }

    [Fact]
    public async Task Should_Not_HaveValidStatus()
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
            await DbOperations.CreateEntityInMemory<FacilitySchedule>(
                InMemoryDataSource.FacilitySchedule1,
                context
            );
            await DbOperations.CreateEntityInMemory<Reservation>(InMemoryDataSource.Reservation1, context);
        }

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        UpdateStatusFromReservationDTO updateReservationTest = new()
        {
            Id = InMemoryDataSource.Reservation1.Id,
            UserId = InMemoryDataSource.UserPlayer.Id,
            Status = (ReservationStatus)999,
        };

        // Act
        Result<Reservation> result = await reservationHandler.UpdateReservationStatus(
            updateReservationTest
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.CreationInvalidStatus, result.Error);
    }

    [Fact]
    public async Task Should_Not_AffectAnyRowFacilitySchedule()
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
            await DbOperations.CreateEntityInMemory<FacilitySchedule>(
                InMemoryDataSource.FacilitySchedule1,
                context
            );
            await DbOperations.CreateEntityInMemory<Reservation>(InMemoryDataSource.Reservation1, context);
        }

        IReservationHandler reservationHandler =
            fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        UpdateStatusFromReservationDTO updateReservationTest = new()
        {
            Id = Guid.NewGuid(),
            UserId = InMemoryDataSource.UserPlayer.Id,
            Status = ReservationStatus.Confirmed,
        };

        // Act
        Result<Reservation> result = await reservationHandler.UpdateReservationStatus(
            updateReservationTest
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.UpdateReservationDoesNotExist, result.Error);
    }
}
