using FourLines.Application.DTOs.Courts;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using FourLines.Tests.Shared.Seed;

namespace FourLines.Tests.Courts;

[Collection(FourLinesCollection.Name)]
public class TestCourtUpdate(FourLinesFixture fixtures)
{
    private static readonly UpdateCourtDTO _updateCourt = new()
    {
        OwnerId = UserSeed.Owner.Id,
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

        UpdateCourtDTO updateCourtDTO = _updateCourt with
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
        UpdateCourtDTO courtWithNoFacility = _updateCourt with { FacilityId = Guid.NewGuid() };

        // Act
        Result<Court> result = await courtHandler.Update(courtWithNoFacility);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.UpdateUnknownFacility, result.Error);
    }

    [Fact]
    public async Task Should_Not_UpdateCourtForAnotherOwnersFacility()
    {
        await using var scope = fixtures.CreateAsyncServiceScope();
        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        Result<Court> result = await courtHandler.Update(
            _updateCourt with
            {
                Id = CourtSeed.ToBeUpdated.Id,
                OwnerId = UserSeed.Player.Id,
            }
        );

        Assert.True(result.IsFailure);
        Assert.Equal(CourtsErrorResults.UpdateUnknownFacility, result.Error);
    }
}
