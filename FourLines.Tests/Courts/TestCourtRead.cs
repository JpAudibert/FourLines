using FourLines.Application.DTOs.Courts;
using FourLines.Application.Handlers;
using FourLines.Domain.Constants;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Courts;

public class TestCourtRead(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
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

    private static Sport _testSport = new()
    {
        Name = "Test Sport",
        Indoor = true,
        StartingPlayersCount = 5,
        MaxPlayersCount = 10,
    };

    private static Court _testCourt1 = new()
    {
        FacilityId = _testFacility.Id,
        SportId = _testSport.Id,
        Name = "Test Court",
        IsActive = true,
    };

    private static Court _testCourt2 = new()
    {
        FacilityId = _testFacility.Id,
        SportId = _testSport.Id,
        Name = "Test Court 2",
        IsActive = true,
    };

    [Fact]
    public async Task Should_GetAllCourts()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(_testRoleOwner);
        await _fixtures.CreateEntityInMemory<User>(_testUser);
        await _fixtures.CreateEntityInMemory<Facility>(_testFacility);
        await _fixtures.CreateEntityInMemory<Sport>(_testSport);
        await _fixtures.CreateEntityInMemory<Court>(_testCourt1);
        await _fixtures.CreateEntityInMemory<Court>(_testCourt2);

        CourtHandler courtHandler =
            _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<IEnumerable<Court>> result = await courtHandler.GetAllCourtsFromFacility(_testFacility.OwnerId, _testFacility.Id);

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetAllCourts()
    {
        // Arrange
        CourtHandler courtHandler =
            _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();
        // Act
        Result<IEnumerable<Court>> result = await courtHandler.GetAllCourtsFromFacility(_testFacility.OwnerId, _testFacility.Id);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.RetrieveGetCourtDoesNotExist, result.Error);
    }

    [Fact]
    public async Task Should_GetFacility()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(_testRoleOwner);
        await _fixtures.CreateEntityInMemory<User>(_testUser);
        await _fixtures.CreateEntityInMemory<Facility>(_testFacility);
        await _fixtures.CreateEntityInMemory<Sport>(_testSport);
        await _fixtures.CreateEntityInMemory<Court>(_testCourt1);
        await _fixtures.CreateEntityInMemory<Court>(_testCourt2);

        CourtHandler courtHandler =
            _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<Court> result = await courtHandler.GetFacility(
            _testFacility.OwnerId, _testCourt1.FacilityId, _testCourt1.Id
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(result.Value.Id, _testCourt1.Id);
        Assert.Equal(result.Value.Name, _testCourt1.Name);
        Assert.Equal(result.Value.IsActive, _testCourt1.IsActive);
        Assert.Equal(result.Value.FacilityId, _testCourt1.FacilityId);
        Assert.Equal(result.Value.SportId, _testCourt1.SportId);
    }

      [Fact]
    public async Task Should_Not_GetFacility()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(_testRoleOwner);
        await _fixtures.CreateEntityInMemory<User>(_testUser);
        await _fixtures.RemoveAllDataFromMemory<Facility>();

        CourtHandler courtHandler =
            _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<Court> result = await courtHandler.GetFacility(
            _testFacility.OwnerId, _testFacility.Id, _testCourt1.Id
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.RetrieveGetCourtDoesNotExist, result.Error);
    }

}
