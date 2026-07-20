using FourLines.Application.Handlers;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Courts;

public class TestCourtRead(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    [Fact]
    public async Task Should_GetAllCourts()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.sport);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.court1);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.court2);

        CourtHandler courtHandler =
            _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<IEnumerable<Court>> result = await courtHandler.GetAllCourtsFromFacility(InMemoryDataSource.facility1.OwnerId, InMemoryDataSource.facility1.Id);

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
        Result<IEnumerable<Court>> result = await courtHandler.GetAllCourtsFromFacility(InMemoryDataSource.facility1.OwnerId, InMemoryDataSource.facility1.Id);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.RetrieveGetCourtDoesNotExist, result.Error);
    }

    [Fact]
    public async Task Should_GetFacility()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.sport);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.court1);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.court2);

        CourtHandler courtHandler =
            _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<Court> result = await courtHandler.GetFacility(
            InMemoryDataSource.facility1.OwnerId, InMemoryDataSource.court1.FacilityId, InMemoryDataSource.court1.Id
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(result.Value.Id, InMemoryDataSource.court1.Id);
        Assert.Equal(result.Value.Name, InMemoryDataSource.court1.Name);
        Assert.Equal(result.Value.IsActive, InMemoryDataSource.court1.IsActive);
        Assert.Equal(result.Value.FacilityId, InMemoryDataSource.court1.FacilityId);
        Assert.Equal(result.Value.SportId, InMemoryDataSource.court1.SportId);
    }

      [Fact]
    public async Task Should_Not_GetFacility()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.RemoveAllDataFromMemory<Facility>();

        CourtHandler courtHandler =
            _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<Court> result = await courtHandler.GetFacility(
            InMemoryDataSource.facility1.OwnerId, InMemoryDataSource.facility1.Id, InMemoryDataSource.court1.Id
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.RetrieveGetCourtDoesNotExist, result.Error);
    }

}
