using FourLines.Application.DTOs.Courts.Interfaces;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using System.Xml.Linq;

namespace FourLines.Tests.Courts;

public record TestCreateCourtDTO : ICreateCourtDTO
{
    public Guid FacilityId { get; init; }
    public bool IsActive { get; init; }
    public string Name { get; init; } = default!;
    public Guid OwnerId { get; init; }
    public Guid SportId { get; init; }
}

[Collection(CourtCollection.Name)]
public class TestCourtCreate(CourtFixture fixtures)
{
    private static readonly TestCreateCourtDTO _createCourtTest = new()
    {
        OwnerId = TestDataSource.UserOwner.Id,
        FacilityId = TestDataSource.DefaultFacility.Id,
        SportId = TestDataSource.DefaultSport.Id,
        Name = "Test Court",
        IsActive = true,
    };

    [Fact]
    public async Task Should_CreateCourt()
    {
        // Arrange
        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<Court> result = await courtHandler.Create(_createCourtTest);

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
        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();
        TestCreateCourtDTO courtWithNoFacility = _createCourtTest with { FacilityId = Guid.NewGuid() };

        // Act
        Result<Court> result = await courtHandler.Create(courtWithNoFacility);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.CreateUnknownFacility, result.Error);
    }

    [Fact]
    public async Task Should_Not_HaveKnownSport()
    {
        // Arrange
        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();
        TestCreateCourtDTO courtWithNoSport = _createCourtTest with { SportId = Guid.NewGuid() };

        // Act
        Result<Court> result = await courtHandler.Create(courtWithNoSport);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.CreateUnknownSport, result.Error);
    }
}
