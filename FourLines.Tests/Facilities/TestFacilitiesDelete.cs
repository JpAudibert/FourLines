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
        await using var context = fixtures.CreateContext();
        Facility testFacility = await DbOperations.CreateRecord<Facility>(TestDataSource.ToBeDeletedFacility, context);

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<bool> result = await facilityHandler.Delete(new DeleteFacilityDTO()
        {
            OwnerId = testFacility.OwnerId,
            FacilityId = testFacility.Id
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
