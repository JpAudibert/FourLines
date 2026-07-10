using FourLines.Application.DTOs.Facilities;
using FourLines.Application.Handlers;
using FourLines.Domain.Constants;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Facilities;

public class TestFacilitiesCreate(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    private static Role _testRoleOwner = new() { Name = RoleConstants.FacilityOwner };
    private static Role _testRolePlayer = new() { Name = RoleConstants.Player };

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

    private static CreateFacilityDTO _createFacilityTest = new()
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
    public async Task Should_CreateFacility()
    {
        // Arrange
        await Task.WhenAll(
            _fixtures.CreateEntityInMemory<Role>(_testRoleOwner),
            _fixtures.CreateEntityInMemory<User>(_testUser)
        );

        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<Facility> result = await facilityHandler.Create(_createFacilityTest);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Facility>(result.Value);
        Assert.Equal(_createFacilityTest.Name, result.Value.Name);
        Assert.Equal(_createFacilityTest.Address, result.Value.Address);
        Assert.Equal(_createFacilityTest.City, result.Value.City);
        Assert.Equal(_createFacilityTest.State, result.Value.State);
        Assert.Equal(_createFacilityTest.ZipCode, result.Value.ZipCode);
        Assert.Equal(_createFacilityTest.RegistrationNumber, result.Value.RegistrationNumber);
        Assert.Equal(_createFacilityTest.OwnerId, result.Value.OwnerId);
    }

    [Fact]
    public async Task Should_Not_CreateFacility()
    {
        // Arrange
        User userPlayer = _testUser;
        userPlayer.RoleId = _testRolePlayer.Id;

        await Task.WhenAll(
            _fixtures.CreateEntityInMemory<Role>(_testRolePlayer),
            _fixtures.CreateEntityInMemory<User>(userPlayer)
        );

        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<Facility> result = await facilityHandler.Create(_createFacilityTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.CreateOwnerDoesNotExists, result.Error);
    }
}
