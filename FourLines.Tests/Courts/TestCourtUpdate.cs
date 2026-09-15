using FourLines.Application.DTOs.Courts.Interfaces;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using FourLines.Tests.Shared.Seed;

namespace FourLines.Tests.Courts;

public record TestUpdateCourtDTO : IUpdateCourtDTO
{
    public Guid FacilityId { get; init; }
    public Guid Id { get; init; }
    public bool IsActive { get; init; }
    public string Name { get; init; } = default!;
    public Guid SportId { get; init; }
    public Money DefaultPrice { get; init; } = default!;
    public int RentingPeriodInMinutes { get; init; }
    public int MaintenancePeriodInMinutes { get; init; }
}

[Collection(FourLinesCollection.Name)]
public class TestCourtUpdate(FourLinesFixture fixtures)
{
    private static readonly TestUpdateCourtDTO _updateCourt = new()
    {
        FacilityId = CourtSeed.ToBeUpdated.FacilityId,
        SportId = CourtSeed.ToBeUpdated.SportId,
        Name = "Test Updated Court",
        IsActive = true,
        DefaultPrice = new Money(100.00m, "BRL"),
        RentingPeriodInMinutes = 50,
        MaintenancePeriodInMinutes = 10,
    };

    [Fact]
    public async Task Should_UpdateCourt()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        TestUpdateCourtDTO updateCourtDTO = _updateCourt with
        {
            Id = CourtSeed.ToBeUpdated.Id,
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
        Assert.Equal(updateCourtDTO.DefaultPrice.Amount, result.Value.DefaultPrice.Amount);
        Assert.Equal(updateCourtDTO.DefaultPrice.Currency, result.Value.DefaultPrice.Currency);
        Assert.Equal(updateCourtDTO.RentingPeriodInMinutes, result.Value.RentingPeriodInMinutes);
        Assert.Equal(
            updateCourtDTO.MaintenancePeriodInMinutes,
            result.Value.MaintenancePeriodInMinutes
        );
    }

    [Fact]
    public async Task Should_Not_FindFacility()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();
        TestUpdateCourtDTO courtWithNoFacility = _updateCourt with { FacilityId = Guid.NewGuid() };

        // Act
        Result<Court> result = await courtHandler.Update(courtWithNoFacility);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.UpdateUnknownFacility, result.Error);
    }
}
