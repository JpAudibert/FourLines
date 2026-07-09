using FourLines.Application.DTOs.Facilities;
using FourLines.Application.Handlers;
using FourLines.Domain.Constants;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Facilities;

public class TestFacilitiesDelete(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
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
    public async Task Should_DeleteFacility()
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

        Result<Facility> resultCreate = await facilityHandler.Create(createFacilityTest);

        // Act
        Result<bool> result = await facilityHandler.Delete(_testUser.Id, resultCreate.Value.Id);

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteFacility()
    {
        // Arrange
        FacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityHandler>();

        // Act
        Result<bool> result = await facilityHandler.Delete(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.False(result.Value);
    }
}
