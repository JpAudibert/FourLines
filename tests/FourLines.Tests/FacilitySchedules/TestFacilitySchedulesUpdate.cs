using FourLines.Application.DTOs.FacilitySchedules;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using FourLines.Tests.Shared.Seed;

namespace FourLines.Tests.FacilitySchedules;

[Collection(FourLinesCollection.Name)]
public class TestFacilitySchedulesUpdate(FourLinesFixture fixtures)
{
    private static readonly UpdateFacilityScheduleDTO _updateFacilityScheduleTest = new()
    {
        OwnerId = UserSeed.Owner.Id,
        Id = FacilityScheduleSeed.ToBeUpdated.Id,
        FacilityId = FacilitySeed.NoSchedules.Id,
        DayOfWeek = DayOfWeek.Friday,
        OpensAt = new TimeOnly(10, 0),
        ClosesAt = new TimeOnly(18, 0),
    };

    [Fact]
    public async Task Should_UpdateFacilitySchedule()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        UpdateFacilityScheduleDTO updateScheduleDTO = _updateFacilityScheduleTest with
        {
            Id = FacilityScheduleSeed.ToBeUpdated.Id,
        };

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        // Act
        Result<FacilitySchedule> result = await facilityScheduleHandler.Update(updateScheduleDTO);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<FacilitySchedule>(result.Value);
        Assert.Equal(_updateFacilityScheduleTest.FacilityId, result.Value.FacilityId);
        Assert.Equal(_updateFacilityScheduleTest.DayOfWeek, result.Value.DayOfWeek);
        Assert.Equal(_updateFacilityScheduleTest.OpensAt, result.Value.OpensAt);
        Assert.Equal(_updateFacilityScheduleTest.ClosesAt, result.Value.ClosesAt);
    }

    [Fact]
    public async Task Should_Not_FindFacility()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();
        UpdateFacilityScheduleDTO scheduleWithNoFacility = _updateFacilityScheduleTest with
        {
            FacilityId = Guid.NewGuid(),
        };

        // Act
        Result<FacilitySchedule> result = await facilityScheduleHandler.Update(
            scheduleWithNoFacility
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitySchedulesErrorResults.UpdateUnknownFacility, result.Error);
    }

    [Fact]
    public async Task Should_Not_AffectAnyRowFacilitySchedule()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();
        UpdateFacilityScheduleDTO scheduleWithNoFacility = _updateFacilityScheduleTest with
        {
            Id = Guid.NewGuid(),
        };

        // Act
        Result<FacilitySchedule> result = await facilityScheduleHandler.Update(
            scheduleWithNoFacility
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitySchedulesErrorResults.UpdateUnknownSchedules, result.Error);
    }

    [Fact]
    public async Task Should_Not_UpdateScheduleForAnotherOwnersFacility()
    {
        await using var scope = fixtures.CreateAsyncServiceScope();
        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        Result<FacilitySchedule> result = await facilityScheduleHandler.Update(
            _updateFacilityScheduleTest with { OwnerId = UserSeed.Player.Id }
        );

        Assert.True(result.IsFailure);
        Assert.Equal(FacilitySchedulesErrorResults.UpdateUnknownFacility, result.Error);
    }
}
