using FourLines.Application.DTOs.Facilities;
using FourLines.Application.Handlers;
using FourLines.Domain.Constants;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Facilities;

public class TestFacilitiesUpdate(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    [Fact]
    public async Task Should_UpdateFacility()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);

        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        UpdateFacilityDTO updateFacilityTest = new()
        {
            Id = InMemoryDataSource.facility1.Id,
            Name = "Test Updated Facility",
            Address = "123 Test St",
            City = "Test City",
            State = "TS",
            ZipCode = "12345",
            RegistrationNumber = "1234567890",
            OwnerId = InMemoryDataSource.userOwner.Id,
        };

        // Act
        Result<Facility> result = await facilityHandler.Update(updateFacilityTest);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Facility>(result.Value);
        Assert.Equal(updateFacilityTest.Name, result.Value.Name);
        Assert.Equal(updateFacilityTest.Address, result.Value.Address);
        Assert.Equal(updateFacilityTest.City, result.Value.City);
        Assert.Equal(updateFacilityTest.State, result.Value.State);
        Assert.Equal(updateFacilityTest.ZipCode, result.Value.ZipCode);
        Assert.Equal(updateFacilityTest.RegistrationNumber, result.Value.RegistrationNumber);
        Assert.Equal(updateFacilityTest.OwnerId, result.Value.OwnerId);
    }

    [Fact]
    public async Task Should_Not_FindOwnerFacility()
    {
        // Arrange
        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        UpdateFacilityDTO updateFacilityTest = new()
        {
            Id = Guid.NewGuid(),
            Name = "Test Updated Facility",
            Address = "123 Test St",
            City = "Test City",
            State = "TS",
            ZipCode = "12345",
            RegistrationNumber = "1234567890",
            OwnerId = Guid.Empty,
        };

        // Act
        Result<Facility> result = await facilityHandler.Update(updateFacilityTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.UpdateEmptyOwnerId, result.Error);
    }

    [Fact]
    public async Task Should_Not_AffectAnyRowFacility()
    {
        // Arrange
        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        UpdateFacilityDTO updateFacilityTest = new()
        {
            Id = Guid.NewGuid(),
            Name = "Test Updated Facility",
            Address = "123 Test St",
            City = "Test City",
            State = "TS",
            ZipCode = "12345",
            RegistrationNumber = "1234567890",
            OwnerId = Guid.NewGuid(),
        };

        // Act
        Result<Facility> result = await facilityHandler.Update(updateFacilityTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.UpdateFacilityDoesNotExist, result.Error);
    }
}
