using FourLines.Application.DTOs.Courts;
using FourLines.Application.Handlers;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Courts;

public class TestCourtUpdate(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    [Fact]
    public async Task Should_UpdateCourt()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.sport);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.court1);

        UpdateCourtDTO updateCourtTest = new()
        {
            Id = InMemoryDataSource.court1.Id,
            OwnerId = InMemoryDataSource.userOwner.Id,
            FacilityId = InMemoryDataSource.facility1.Id,
            SportId = InMemoryDataSource.sport.Id,
            Name = "Test Updated Court",
            IsActive = true,
        };

        CourtHandler courtHandler =
            _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<Court> result = await courtHandler.Update(updateCourtTest);

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
            Id = InMemoryDataSource.court1.Id,
            FacilityId = Guid.NewGuid(),
            SportId = InMemoryDataSource.sport.Id,
            Name = "Test Updated Court",
            OwnerId = InMemoryDataSource.userOwner.Id,
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
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);

        CourtHandler courtHandler =
            _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        UpdateCourtDTO updateCourtTest = new()
        {
            Id = InMemoryDataSource.court1.Id,
            FacilityId = InMemoryDataSource.facility1.Id,
            SportId = InMemoryDataSource.sport.Id,
            Name = "Test Updated Court",
            IsActive = true,
            OwnerId = InMemoryDataSource.userOwner.Id,
        };

        // Act
        Result<Court> result = await courtHandler.Update(updateCourtTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.UpdateCourtDoesNotExist, result.Error);
    }
}
