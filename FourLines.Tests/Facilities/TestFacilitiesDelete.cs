using FourLines.Application.DTOs.Facilities;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Facilities;

[Collection(FourLinesCollection.Name)]
public class TestFacilitiesDelete(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_DeleteFacility()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<bool> result = await facilityHandler.Delete(new DeleteFacilityDTO()
        {
            OwnerId = TestDataSource.ToBeDeletedFacility.OwnerId,
            FacilityId = TestDataSource.ToBeDeletedFacility.Id
        });

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteFacility()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

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
