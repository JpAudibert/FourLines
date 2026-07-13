using FourLines.Application.DTOs.Courts;
using FourLines.Application.Handlers;
using FourLines.Domain.Constants;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Courts;

public class TestCourtCreate(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
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

    private static CreateCourtDTO _createCourtTest = new()
    {
        OwnerId = _testUser.Id,
        FacilityId = _testFacility.Id,
        SportId = _testSport.Id,
        Name = "Test Court",
        IsActive = true,
    };

    [Fact]
    public async Task Should_CreateCourt()
    {
        // Arrange
        
        await _fixtures.CreateEntityInMemory<Role>(_testRoleOwner);
        await _fixtures.CreateEntityInMemory<User>(_testUser);
        await _fixtures.CreateEntityInMemory<Facility>(_testFacility);
        await _fixtures.CreateEntityInMemory<Sport>(_testSport);

        CourtHandler courtHandler = _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<Domain.Models.Court> result = await courtHandler.Create(_createCourtTest);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Court>(result.Value);
        Assert.Equal(_createCourtTest.Name, result.Value.Name);
        Assert.Equal(_createCourtTest.FacilityId, result.Value.FacilityId);
        Assert.Equal(_createCourtTest.SportId, result.Value.SportId);
        Assert.Equal(_createCourtTest.IsActive, result.Value.IsActive);
    }

    [Fact]
    public async Task Should_Not_HaveFacilityToCreateCourt()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(_testRoleOwner);
        await _fixtures.CreateEntityInMemory<User>(_testUser);
        await _fixtures.CreateEntityInMemory<Sport>(_testSport);
        await _fixtures.RemoveDataFromMemory<Facility>(_testFacility.Id);

        CourtHandler courtHandler = _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<Court> result = await courtHandler.Create(_createCourtTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.CreateUnknownFacility, result.Error);
    }
}
