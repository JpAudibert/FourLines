using FourLines.Application.Handlers;
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

        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<bool> result = await reservationHandler.Delete(
            InMemoryDataSource.UserPlayer.Id,
            InMemoryDataSource.Reservation1.Id
        );

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteReservation()
    {
        // Arrange
        ReservationHandler reservationHandler =
            _fixtures.ServiceProvider.GetRequiredService<ReservationHandler>();

        // Act
        Result<bool> result = await reservationHandler.Delete(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.False(result.Value);
    }
}
