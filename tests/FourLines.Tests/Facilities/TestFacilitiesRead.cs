using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Infrastructure.Contexts;
using FourLines.Tests.Shared;
using FourLines.Tests.Shared.Seed;

namespace FourLines.Tests.Facilities;

[Collection(FourLinesCollection.Name)]
public class TestFacilitiesRead(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_GetAllFacilities()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();
        FourLinesContext context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<IEnumerable<Facility>> result = await facilityHandler.GetAllFacilities();

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(context.Facilities.Count(), result.Value.Count());
    }

    [Fact]
    public async Task Should_GetFacilities()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        FourLinesContext context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<IEnumerable<Facility>> result = await facilityHandler.GetFacilitiesFromOwner(
            FacilitySeed.Default.OwnerId
        );

        // Assert
        int ownersFacilities = context
            .Facilities.Where(f => f.OwnerId == FacilitySeed.Default.OwnerId)
            .Count();

        Assert.NotEmpty(result.Value);
        Assert.Equal(ownersFacilities, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetFacilities()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<IEnumerable<Facility>> result = await facilityHandler.GetFacilitiesFromOwner(
            Guid.NewGuid()
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.RetrieveOwnerDoesNotExists, result.Error);
    }

    [Fact]
    public async Task Should_GetFacility()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<Facility> result = await facilityHandler.GetFacilityFromOwner(
            FacilitySeed.Default.OwnerId,
            FacilitySeed.Default.Id
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(FacilitySeed.Default.Name, result.Value.Name);
        Assert.Equal(FacilitySeed.Default.Address, result.Value.Address);
        Assert.Equal(FacilitySeed.Default.City, result.Value.City);
        Assert.Equal(FacilitySeed.Default.State, result.Value.State);
        Assert.Equal(FacilitySeed.Default.ZipCode, result.Value.ZipCode);
        Assert.Equal(FacilitySeed.Default.RegistrationNumber, result.Value.RegistrationNumber);
        Assert.Equal(FacilitySeed.Default.OwnerId, result.Value.OwnerId);
    }

    [Fact]
    public async Task Should_Not_GetOwnerFacility()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<Facility> result = await facilityHandler.GetFacilityFromOwner(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.RetrieveOwnerDoesNotExists, result.Error);
    }

    [Fact]
    public async Task Should_Not_GetFacility()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        // Act
        Result<Facility> result = await facilityHandler.GetFacilityFromOwner(
            FacilitySeed.Default.OwnerId,
            Guid.NewGuid()
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.RetrieveFacilityDoesNotExist, result.Error);
    }
}
