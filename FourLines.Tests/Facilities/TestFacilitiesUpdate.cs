using FourLines.Application.DTOs.Facilities;
using FourLines.Application.Handlers;
using FourLines.Domain.Constants;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Facilities;

public class TestFacilitiesUpdate(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    private static Role _testRoleOwner = new() { Name = RoleConstants.FacilityOwner };

    private static User _testUser = new()
    {
        RoleId = _testRoleOwner.Id,
        Name = "John Doe",
        Email = "john.doe@example.com",
        PasswordHash = "Password123!",
        Birthday = new DateOnly(1970, 1, 1),
        Phone = "55 54 9 9999-9999",
        RegistrationNumber = "383.975.210-89",
    };

    private static Facility _testFacility = new()
    {
        Name = "Test Facility",
        Address = "123 Test St",
        City = "Test City",
        State = "TS",
        ZipCode = "12345",
        RegistrationNumber = "1234567890",
        OwnerId = _testUser.Id,
    };

    [Fact]
    public async Task Should_UpdateFacility()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(_testRoleOwner);
        await _fixtures.CreateEntityInMemory<User>(_testUser);
        await _fixtures.CreateEntityInMemory<Facility>(_testFacility);

        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        UpdateFacilityDTO updateFacilityTest = new()
        {
            Id = _testFacility.Id,
            Name = "Test Updated Facility",
            Address = "123 Test St",
            City = "Test City",
            State = "TS",
            ZipCode = "12345",
            RegistrationNumber = "1234567890",
            OwnerId = _testUser.Id,
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
