using FourLines.Application.DTOs.Facilities;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using FourLines.Tests.Shared.Seed;

namespace FourLines.Tests.Facilities;

[Collection(FourLinesCollection.Name)]
public class TestFacilitiesUpdate(FourLinesFixture fixtures)
{
    private static readonly UpdateFacilityDTO _updateFacilityTest = new()
    {
        Id = FacilitySeed.ToBeUpdated.Id,
        Name = "Test Updated Facility",
        Address = "123 Test St",
        City = "Test City",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "1111111111",
        OwnerId = FacilitySeed.ToBeUpdated.OwnerId,
        TimeZoneId = "America/Sao_Paulo"
    };

    [Fact]
    public async Task Should_UpdateFacility()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        UpdateFacilityDTO updateFacilityDTO = _updateFacilityTest with
        {
            Id = FacilitySeed.ToBeUpdated.Id,
        };

        // Act
        Result<Facility> result = await facilityHandler.Update(updateFacilityDTO);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Facility>(result.Value);
        Assert.Equal(updateFacilityDTO.Name, result.Value.Name);
        Assert.Equal(updateFacilityDTO.Address, result.Value.Address);
        Assert.Equal(updateFacilityDTO.City, result.Value.City);
        Assert.Equal(updateFacilityDTO.State, result.Value.State);
        Assert.Equal(updateFacilityDTO.ZipCode, result.Value.ZipCode);
        Assert.Equal(updateFacilityDTO.RegistrationNumber, result.Value.RegistrationNumber);
        Assert.Equal(updateFacilityDTO.OwnerId, result.Value.OwnerId);
    }

    [Fact]
    public async Task Should_Not_FindOwnerFacility()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();
        UpdateFacilityDTO facilityWithNoOwnerId = _updateFacilityTest with
        {
            OwnerId = Guid.Empty,
        };

        // Act
        Result<Facility> result = await facilityHandler.Update(facilityWithNoOwnerId);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.UpdateEmptyOwnerId, result.Error);
    }

    [Fact]
    public async Task Should_Not_AffectAnyRowFacility()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();
        UpdateFacilityDTO unknownFacility = _updateFacilityTest with { Id = Guid.NewGuid() };

        // Act
        Result<Facility> result = await facilityHandler.Update(unknownFacility);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.UpdateFacilityDoesNotExist, result.Error);
    }

    [Fact]
    public async Task Should_Not_UpdateAnotherOwnersFacility()
    {
        await using var scope = fixtures.CreateAsyncServiceScope();
        IFacilityHandler facilityHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        Result<Facility> result = await facilityHandler.Update(
            _updateFacilityTest with { OwnerId = UserSeed.Player.Id }
        );

        Assert.True(result.IsFailure);
        Assert.Equal(FacilitiesErrorResults.UpdateFacilityDoesNotExist, result.Error);
    }
}
