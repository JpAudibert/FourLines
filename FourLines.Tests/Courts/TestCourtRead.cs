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
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.Court1);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.Court2);

        CourtHandler courtHandler =
            _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<IEnumerable<Court>> result = await courtHandler.GetAllCourtsFromFacility(InMemoryDataSource.Facility1.OwnerId, InMemoryDataSource.Facility1.Id);

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
        Result<IEnumerable<Court>> result = await courtHandler.GetAllCourtsFromFacility(InMemoryDataSource.Facility1.OwnerId, InMemoryDataSource.Facility1.Id);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.RetrieveGetCourtDoesNotExist, result.Error);
    }

    [Fact]
    public async Task Should_GetFacility()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.Court1);
        await _fixtures.CreateEntityInMemory<Court>(InMemoryDataSource.Court2);

        CourtHandler courtHandler =
            _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<Court> result = await courtHandler.GetFacility(
            InMemoryDataSource.Facility1.OwnerId, InMemoryDataSource.Court1.FacilityId, InMemoryDataSource.Court1.Id
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(result.Value.Id, InMemoryDataSource.Court1.Id);
        Assert.Equal(result.Value.Name, InMemoryDataSource.Court1.Name);
        Assert.Equal(result.Value.IsActive, InMemoryDataSource.Court1.IsActive);
        Assert.Equal(result.Value.FacilityId, InMemoryDataSource.Court1.FacilityId);
        Assert.Equal(result.Value.SportId, InMemoryDataSource.Court1.SportId);
    }

      [Fact]
    public async Task Should_Not_GetFacility()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner);
        await _fixtures.RemoveAllDataFromMemory<Facility>();

        CourtHandler courtHandler =
            _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<Court> result = await courtHandler.GetFacility(
            InMemoryDataSource.Facility1.OwnerId, InMemoryDataSource.Facility1.Id, InMemoryDataSource.Court1.Id
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.RetrieveGetCourtDoesNotExist, result.Error);
    }

}
