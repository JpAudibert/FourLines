using FourLines.Application.DTOs.Facilities;
using FourLines.Application.Handlers;
using FourLines.Domain.Constants;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests;

public class FacilitiesRegisterAndOwnershipTests(InMemoryFixtures fixtures)
    : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    private static Role _testRole = new() { Name = RoleConstants.FacilityOwner };

    private static User _testUser = new()
    {
        RoleId = _testRole.Id,
        Name = "John Doe",
        Email = "john.doe@example.com",
        PasswordHash = "Password123!",
        Birthday = new DateOnly(1970, 1, 1),
        Phone = "55 54 9 9999-9999",
        RegistrationNumber = "383.975.210-89",
    };

    [Fact]
    public async Task Should_CreateFacility()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(_testRole);
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

        // Act
        Result<Facility> result = await facilityHandler.Create(createFacilityTest);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Facility>(result.Value);
        Assert.Equal(createFacilityTest.Name, result.Value.Name);
        Assert.Equal(createFacilityTest.Address, result.Value.Address);
        Assert.Equal(createFacilityTest.City, result.Value.City);
        Assert.Equal(createFacilityTest.State, result.Value.State);
        Assert.Equal(createFacilityTest.ZipCode, result.Value.ZipCode);
        Assert.Equal(createFacilityTest.RegistrationNumber, result.Value.RegistrationNumber);
        Assert.Equal(createFacilityTest.OwnerId, result.Value.OwnerId);
    }
}
