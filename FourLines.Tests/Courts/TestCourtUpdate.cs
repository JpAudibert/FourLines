using FourLines.Application.DTOs.Courts.Interfaces;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Courts;

public record TestUpdateCourtDTO : IUpdateCourtDTO
{
    public Guid FacilityId { get; init; }
    public Guid Id { get; init; }
    public bool IsActive { get; init; }
    public string Name { get; init; } = default!;
    public Guid SportId { get; init; }
}

[Collection(CourtCollection.Name)]
public class TestCourtUpdate(CourtFixture fixtures)
{
    private static readonly TestUpdateCourtDTO _updateCourt = new()
    {
        Id = TestDataSource.Court3.Id,
        FacilityId = TestDataSource.Facility2.Id,
        SportId = TestDataSource.DefaultSport.Id,
        Name = "Test Updated Court",
        IsActive = true,
    };

    [Fact]
    public async Task Should_UpdateCourt()
    {
        // Arrange
        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<Court> result = await courtHandler.Update(_updateCourt);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Court>(result.Value);
        Assert.Equal(_updateCourt.Name, result.Value.Name);
        Assert.Equal(_updateCourt.FacilityId, result.Value.FacilityId);
        Assert.Equal(_updateCourt.SportId, result.Value.SportId);
        Assert.Equal(_updateCourt.IsActive, result.Value.IsActive);
    }

    [Fact]
    public async Task Should_Not_FindFacility()
    {
        // Arrange
        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();
        TestUpdateCourtDTO courtWithNoFacility = _updateCourt with { FacilityId = Guid.NewGuid() };

        // Act
        Result<Court> result = await courtHandler.Update(courtWithNoFacility);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.UpdateUnknownFacility, result.Error);
    }
}
