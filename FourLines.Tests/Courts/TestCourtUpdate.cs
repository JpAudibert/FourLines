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

[Collection(FourLinesCollection.Name)]
public class TestCourtUpdate(FourLinesFixture fixtures)
{
    private static readonly TestUpdateCourtDTO _updateCourt = new()
    {
        Id = TestDataSource.Court3.Id,
        FacilityId = TestDataSource.ToBeUpdatedCourt.FacilityId,
        SportId = TestDataSource.ToBeUpdatedCourt.SportId,
        Name = "Test Updated Court",
        IsActive = true,
    };

    [Fact]
    public async Task Should_UpdateCourt()
    {
        // Arrange
        await using var context = fixtures.CreateContext();
        Court testCourt = await DbOperations.CreateRecord<Court>(TestDataSource.ToBeUpdatedCourt, context);

        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        TestUpdateCourtDTO updateCourtDTO = _updateCourt with
        {
            Id = testCourt.Id,
            Name = "Updated Court Name",
        };

        // Act
        Result<Court> result = await courtHandler.Update(updateCourtDTO);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Court>(result.Value);
        Assert.Equal(updateCourtDTO.Name, result.Value.Name);
        Assert.Equal(updateCourtDTO.FacilityId, result.Value.FacilityId);
        Assert.Equal(updateCourtDTO.SportId, result.Value.SportId);
        Assert.Equal(updateCourtDTO.IsActive, result.Value.IsActive);

        await DbOperations.RemoveRecord<Court>(testCourt.Id, context);
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
