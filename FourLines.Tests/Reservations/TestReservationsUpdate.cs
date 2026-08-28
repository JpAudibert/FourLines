using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Reservations;

public class TestReservationsUpdate(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    [Fact]
    public async Task Should_UpdateFacilitySchedule()
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

        IReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        UpdateStatusFromReservationDTO updateReservationTest = new()
        {
            Id = InMemoryDataSource.Reservation1.Id,
            UserId = InMemoryDataSource.UserPlayer.Id,
            Status = ReservationStatus.Confirmed,
        };

        // Act
        Result<Reservation> result = await reservationHandler.Update(
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

        IReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        UpdateStatusFromReservationDTO updateReservationTest = new()
        {
            Id = InMemoryDataSource.Reservation1.Id,
            UserId = InMemoryDataSource.UserPlayer.Id,
            Status = (ReservationStatus) 999,
        };

        // Act
        Result<Reservation> result = await reservationHandler.Update(
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

        IReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

        UpdateStatusFromReservationDTO updateReservationTest = new()
        {
            Id = Guid.NewGuid(),
            UserId = InMemoryDataSource.UserPlayer.Id,
            Status = ReservationStatus.Confirmed,
        };

        // Act
        Result<Reservation> result = await reservationHandler.Update(
            updateReservationTest
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(ReservationsErrorResults.UpdateReservationDoesNotExist, result.Error);
    }
}
