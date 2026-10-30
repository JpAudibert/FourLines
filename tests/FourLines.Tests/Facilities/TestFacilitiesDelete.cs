using FourLines.Application.DTOs.Facilities;
using FourLines.Application.Interfaces;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;
using FourLines.Tests.Shared.Seed;

namespace FourLines.Tests.Facilities;

[Collection(FourLinesCollection.Name)]
public class TestFacilitiesDelete(FourLinesFixture fixtures)
{
    private readonly DeleteFacilityDTO _deleteFacility = new()
    {
        OwnerId = FacilitySeed.ToBeDeleted.OwnerId,
        FacilityId = FacilitySeed.ToBeDeleted.Id,
    };

    [Fact]
    public async Task Should_DeleteFacility()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<bool> result = await facilityHandler.Delete(_deleteFacility);

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
        Result<bool> result = await facilityHandler.Delete(
            _deleteFacility with
            {
                OwnerId = Guid.NewGuid(),
                FacilityId = Guid.NewGuid(),
            }
        );

        // Assert
        Assert.False(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteAnotherOwnersFacility()
    {
        await using var scope = fixtures.CreateAsyncServiceScope();
        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        Result<bool> result = await facilityHandler.Delete(
            _deleteFacility with { OwnerId = UserSeed.Player.Id }
        );

        Assert.True(result.IsFailure);
    }
}
