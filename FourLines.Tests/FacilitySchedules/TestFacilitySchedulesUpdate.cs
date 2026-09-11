using FourLines.Application.DTOs.FacilitySchedules.Interfaces;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.FacilitySchedules;

public record TestUpdateScheduleDTO : IUpdateFacilityScheduleDTO
{
    public TimeOnly ClosesAt { get; init; }
    public DayOfWeek DayOfWeek { get; init; }
    public Guid FacilityId { get; init; }
    public Guid Id { get; init; }
    public TimeOnly OpensAt { get; init; }
}

[Collection(FourLinesCollection.Name)]
public class TestFacilitySchedulesUpdate(FourLinesFixture fixtures)
{
    private static readonly TestUpdateScheduleDTO _updateFacilityScheduleTest = new()
    {
        Id = TestDataSource.ToBeUpdatedFacilitySchedule.Id,
        FacilityId = TestDataSource.DefaultNoSchedulesFacility.Id,
        DayOfWeek = DayOfWeek.Friday,
        OpensAt = new TimeOnly(10, 0),
        ClosesAt = new TimeOnly(18, 0),
    };

    [Fact]
    public async Task Should_UpdateFacilitySchedule()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        TestUpdateScheduleDTO updateScheduleDTO = _updateFacilityScheduleTest with
        {
            Id = TestDataSource.ToBeUpdatedFacilitySchedule.Id
        };

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        // Act
        Result<FacilitySchedule> result = await facilityScheduleHandler.Update(
            updateScheduleDTO
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<FacilitySchedule>(result.Value);
        Assert.Equal(_updateFacilityScheduleTest.FacilityId, result.Value.FacilityId);
        Assert.Equal(_updateFacilityScheduleTest.DayOfWeek, result.Value.DayOfWeek);
        Assert.Equal(_updateFacilityScheduleTest.OpensAt, result.Value.OpensAt);
        Assert.Equal(_updateFacilityScheduleTest.ClosesAt, result.Value.ClosesAt);

        //await DbOperations.RemoveRecord<FacilitySchedule>(result.Value.Id, fixtures.Context);
    }

    [Fact]
    public async Task Should_Not_FindFacility()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();
        TestUpdateScheduleDTO scheduleWithNoFacility = _updateFacilityScheduleTest with { FacilityId = Guid.NewGuid() };

        // Act
        Result<FacilitySchedule> result = await facilityScheduleHandler.Update(scheduleWithNoFacility);

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
        TestUpdateScheduleDTO scheduleWithNoFacility = _updateFacilityScheduleTest with { Id = Guid.NewGuid() };

        // Act
        Result<FacilitySchedule> result = await facilityScheduleHandler.Update(scheduleWithNoFacility);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitySchedulesErrorResults.UpdateUnknownSchedules, result.Error);
    }
}
