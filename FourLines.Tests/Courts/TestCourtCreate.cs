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
        OwnerId = InMemoryDataSource.UserOwner.Id,
        FacilityId = InMemoryDataSource.Facility1.Id,
        SportId = InMemoryDataSource.TestSport.Id,
        Name = "Test Court",
        IsActive = true,
    };

    [Fact]
    public async Task Should_CreateCourt()
    {
        // Arrange

        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport);

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
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner);
        await _fixtures.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport);
        await _fixtures.RemoveDataFromMemory<Facility>(InMemoryDataSource.Facility1.Id);

        CourtHandler courtHandler = _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<Court> result = await courtHandler.Create(_createCourtTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.CreateUnknownFacility, result.Error);
    }

    [Fact]
    public async Task Should_Not_HaveKnownSport()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1);
        await _fixtures.RemoveAllDataFromMemory<Sport>();

        CourtHandler courtHandler = _fixtures.ServiceProvider.GetRequiredService<CourtHandler>();

        // Act
        Result<Court> result = await courtHandler.Create(_createCourtTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.CreateUnknownSport, result.Error);
    }
}
