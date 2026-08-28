using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Reservations;

public class TestReservationsDelete(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    [Fact]
    public async Task Should_DeleteReservation()
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

        // Act
        Result<bool> result = await reservationHandler.Delete(new DeleteReservationDTO
        {
            UserId = InMemoryDataSource.UserPlayer.Id,
            ReservationId = InMemoryDataSource.Reservation1.Id
        });

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteReservation()
    {
        // Arrange
        IReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();

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
