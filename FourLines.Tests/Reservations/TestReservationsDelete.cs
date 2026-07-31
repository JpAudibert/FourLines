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
    public async Task Should_DeleteFacilitySchedule()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);

        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<bool> result = await facilityHandler.Delete(
            InMemoryDataSource.userOwner.Id,
            InMemoryDataSource.facility1.Id
        );

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteFacilitySchedule()
    {
        // Arrange
        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<bool> result = await facilityHandler.Delete(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.False(result.Value);
    }
}
