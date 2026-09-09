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

[Collection(FacilitySchedulesCollection.Name)]
public class TestFacilitySchedulesUpdate(FacilitySchedulesFixture fixtures)
{
    private static readonly TestUpdateScheduleDTO _updateFacilityScheduleTest = new()
    {
        Id = TestDataSource.FacilitySchedule4.Id,
        FacilityId = TestDataSource.Facility3.Id,
        DayOfWeek = TestDataSource.FacilitySchedule4.DayOfWeek,
        OpensAt = new TimeOnly(10, 0),
        ClosesAt = new TimeOnly(18, 0),
    };

    [Fact]
    public async Task Should_UpdateFacilitySchedule()
    {
        // Arrange
        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        // Act
        Result<FacilitySchedule> result = await facilityScheduleHandler.Update(
            _updateFacilityScheduleTest
        );

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
