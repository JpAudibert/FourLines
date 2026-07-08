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
    private static Role _testRolePlayer = new() { Name = RoleConstants.Player };

    private static User _testUser = new()
    {
        RoleId = Guid.Empty,
        Name = "John Doe",
        Email = "john.doe@example.com",
        PasswordHash = "Password123!",
        Birthday = new DateOnly(1970, 1, 1),
        Phone = "55 54 9 9999-9999",
        RegistrationNumber = "383.975.210-89",
    };

    [Fact]
    public async Task Should_UpdateFacility()
    {
        // Arrange
        Role roleOwner = await _fixtures.CreateEntityInMemory<Role>(_testRoleOwner);

        _testUser.RoleId = roleOwner.Id;
        await _fixtures.CreateEntityInMemory<User>(_testUser);

        CreateFacilityDTO createFacilityTest = new()
        {
            Name = "Test Facility",
            Address = "123 Test St",
            City = "Test City",
            State = "TS",
            ZipCode = "12345",
            RegistrationNumber = "1234567890",
            OwnerId = _testUser.Id,
        };

        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        Result<Facility> result = await facilityHandler.Create(createFacilityTest);

        UpdateFacilityDTO updateFacilityTest = new()
        {
            Id = result.Value.Id,
            Name = "Test Updated Facility",
            Address = "123 Test St",
            City = "Test City",
            State = "TS",
            ZipCode = "12345",
            RegistrationNumber = "1234567890",
            OwnerId = _testUser.Id,
        };

        // Act
        result = await facilityHandler.Update(updateFacilityTest); //TODO: UpdateFacilityDTO nao esta funcionado corretamente -> testar

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
    public async Task Should_Not_UpdateFacility()
    {
        // Arrange
        User userPlayer = _testUser;
        userPlayer.RoleId = _testRolePlayer.Id;

        await Task.WhenAll(
            _fixtures.CreateEntityInMemory<Role>(_testRolePlayer),
            _fixtures.CreateEntityInMemory<User>(userPlayer)
        );

        CreateFacilityDTO createFacilityTest = new()
        {
            Name = "Test Facility",
            Address = "123 Test St",
            City = "Test City",
            State = "TS",
            ZipCode = "12345",
            RegistrationNumber = "1234567890",
            OwnerId = _testUser.Id,
        };

        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<Facility> result = await facilityHandler.Create(createFacilityTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.CreateOwnerDoesNotExists, result.Error);
    }
}
