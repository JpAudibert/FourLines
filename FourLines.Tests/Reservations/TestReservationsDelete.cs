using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Reservations;

public class TestReservationsDelete(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    [Fact]
    public async Task Should_DeleteReservation()
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
