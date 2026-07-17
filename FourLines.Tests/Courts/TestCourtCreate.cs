using FourLines.Application.DTOs.Courts;
using FourLines.Application.Handlers;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.Courts;

public class TestCourtCreate(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    private static CreateCourtDTO _createCourtTest = new()
    {
        OwnerId = InMemoryDataSource.userOwner.Id,
        FacilityId = InMemoryDataSource.facility1.Id,
        SportId = InMemoryDataSource.sport.Id,
        Name = "Test Court",
        IsActive = true,
    };

    [Fact]
    public async Task Should_CreateCourt()
    {
        // Arrange
        
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.sport);

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
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.roleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.userOwner);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.sport);
        await _fixtures.RemoveDataFromMemory<Facility>(InMemoryDataSource.facility1.Id);

        CourtHandler courtHandler = _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<Court> result = await courtHandler.Create(_createCourtTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.CreateUnknownFacility, result.Error);
    }
}
