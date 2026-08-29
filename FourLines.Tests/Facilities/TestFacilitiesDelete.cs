using FourLines.Application.DTOs.Facilities;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Facilities;

public class TestFacilitiesDelete(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    [Fact]
    public async Task Should_DeleteFacility()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1);

        IFacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<bool> result = await facilityHandler.Delete(new DeleteFacilityDTO()
        {
            OwnerId = InMemoryDataSource.UserOwner.Id,
            FacilityId = InMemoryDataSource.Facility1.Id
        });

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteFacility()
    {
        // Arrange
        IFacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<bool> result = await facilityHandler.Delete(new DeleteFacilityDTO()
        {
            OwnerId = Guid.NewGuid(),
            FacilityId = Guid.NewGuid()
        });

        // Assert
        Assert.False(result.Value);
    }
}
