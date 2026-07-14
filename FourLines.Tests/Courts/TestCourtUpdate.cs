using FourLines.Application.DTOs.Courts;
using FourLines.Application.Handlers;
using FourLines.Domain.Constants;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Courts;

public class TestCourtUpdate(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
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

    private static Court _testCourt = new()
    {
        FacilityId = _testFacility.Id,
        SportId = _testSport.Id,
        Name = "Test Court",
        IsActive = true,
    };

    [Fact]
    public async Task Should_UpdateCourt()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(_testRoleOwner);
        await _fixtures.CreateEntityInMemory<User>(_testUser);
        await _fixtures.CreateEntityInMemory<Facility>(_testFacility);
        await _fixtures.CreateEntityInMemory<Sport>(_testSport);
        await _fixtures.CreateEntityInMemory<Court>(_testCourt);

        UpdateCourtDTO updateCourtTest = new()
        {
            Id = _testCourt.Id,
            OwnerId = _testUser.Id,
            FacilityId = _testFacility.Id,
            SportId = _testSport.Id,
            Name = "Test Updated Court",
            IsActive = true,
        };

        CourtHandler courtHandler =
            _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<Court> result = await courtHandler.Update(updateCourtTest); //TODO: court does not have ownerId, so we need to find a way to pass it to the update method

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Court>(result.Value);
        Assert.Equal(updateCourtTest.Name, result.Value.Name);
        Assert.Equal(updateCourtTest.FacilityId, result.Value.FacilityId);
        Assert.Equal(updateCourtTest.SportId, result.Value.SportId);
        Assert.Equal(updateCourtTest.IsActive, result.Value.IsActive);
    }

    [Fact]
    public async Task Should_Not_FindFacility()
    {
        // Arrange
        CourtHandler courtHandler =
            _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        UpdateCourtDTO updateCourtTest = new()
        {
            Id = _testCourt.Id,
            FacilityId = Guid.NewGuid(),
            SportId = _testSport.Id,
            Name = "Test Updated Court",
            OwnerId = _testUser.Id,
        };

        // Act
        Result<Court> result = await courtHandler.Update(updateCourtTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.UpdateUnknownFacility, result.Error);
    }

    [Fact]
    public async Task Should_Not_AffectAnyRowFacility()
    {
        // Arrange
        await _fixtures.RemoveAllDataFromMemory<Court>();
        await _fixtures.CreateEntityInMemory<Role>(_testRoleOwner);
        await _fixtures.CreateEntityInMemory<User>(_testUser);
        await _fixtures.CreateEntityInMemory<Facility>(_testFacility);

        CourtHandler courtHandler =
            _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        UpdateCourtDTO updateCourtTest = new()
        {
            Id = _testCourt.Id,
            FacilityId = _testFacility.Id,
            SportId = _testSport.Id,
            Name = "Test Updated Court",
            IsActive = true,
            OwnerId = _testUser.Id,
        };

        // Act
        Result<Court> result = await courtHandler.Update(updateCourtTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.UpdateCourtDoesNotExist, result.Error);
    }
}
