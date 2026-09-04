using FourLines.Application.DTOs.Facilities;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Facilities;

public class TestFacilitiesDelete(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    [Fact]
    public async Task Should_DeleteFacility()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
        }

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

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
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

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
