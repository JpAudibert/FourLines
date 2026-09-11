using FourLines.Application.DTOs.FacilitySchedules.Interfaces;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.FacilitySchedules;

public record TestCreateFacilityScheduleDTO : ICreateFacilityScheduleDTO
{
    public TimeOnly ClosesAt { get; init; }
    public DayOfWeek DayOfWeek { get; init; }
    public Guid FacilityId { get; init; }
    public TimeOnly OpensAt { get; init; }
}

[Collection(FourLinesCollection.Name)]
public class TestFacilitySchedulesCreate(FourLinesFixture fixtures)
{
    private readonly static TestCreateFacilityScheduleDTO _createFacilityScheduleTest1 = new()
    {
        FacilityId = TestDataSource.DefaultNoSchedulesFacility.Id,
        DayOfWeek = DayOfWeek.Monday,
        OpensAt = new TimeOnly(9, 0),
        ClosesAt = new TimeOnly(17, 0),
    };

    [Fact]
    public async Task Should_CreateFacilitySchedule()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityScheduleHandler facilityScheduleHandler = fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        // Act
        Result<FacilitySchedule> result = await facilityScheduleHandler.Create(
            _createFacilityScheduleTest1
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<FacilitySchedule>(result.Value);
        Assert.Equal(_createFacilityScheduleTest1.FacilityId, result.Value.FacilityId);
        Assert.Equal(_createFacilityScheduleTest1.DayOfWeek, result.Value.DayOfWeek);
        Assert.Equal(_createFacilityScheduleTest1.OpensAt, result.Value.OpensAt);
        Assert.Equal(_createFacilityScheduleTest1.ClosesAt, result.Value.ClosesAt);
    }

    [Fact]
    public async Task Should_Not_CreateFacilitySchedule()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();
        TestCreateFacilityScheduleDTO scheduleWithUnknownFacility =
            _createFacilityScheduleTest1 with { FacilityId = Guid.NewGuid() };

        // Act
        Result<FacilitySchedule> result = await facilityScheduleHandler.Create(
            scheduleWithUnknownFacility
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitySchedulesErrorResults.CreateFacilitySchedules, result.Error);
    }

    [Fact]
    public async Task Should_CreateMultipleFacilitySchedule()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        TestCreateFacilityScheduleDTO schedule2 = _createFacilityScheduleTest1 with
        {
            FacilityId = TestDataSource.DefaultNoSchedulesFacility.Id,
            DayOfWeek = DayOfWeek.Tuesday
        };
        TestCreateFacilityScheduleDTO schedule3 = _createFacilityScheduleTest1 with
        {
            FacilityId = TestDataSource.DefaultNoSchedulesFacility.Id,
            DayOfWeek = DayOfWeek.Wednesday
        };

        List<ICreateFacilityScheduleDTO> newSchedules =
        [
            schedule2,
            schedule3
        ];

        // Act
        Result<IEnumerable<FacilitySchedule>> result = await facilityScheduleHandler.CreateMultiple(
            newSchedules
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<FacilitySchedule>(result.Value.ElementAt(0));
        Assert.IsType<FacilitySchedule>(result.Value.ElementAt(1));

        Assert.Equal(newSchedules[0].FacilityId, result.Value.ElementAt(0).FacilityId);
        Assert.Equal(newSchedules[0].DayOfWeek, result.Value.ElementAt(0).DayOfWeek);
        Assert.Equal(newSchedules[0].OpensAt, result.Value.ElementAt(0).OpensAt);
        Assert.Equal(newSchedules[0].ClosesAt, result.Value.ElementAt(0).ClosesAt);

        Assert.Equal(newSchedules[1].FacilityId, result.Value.ElementAt(1).FacilityId);
        Assert.Equal(newSchedules[1].DayOfWeek, result.Value.ElementAt(1).DayOfWeek);
        Assert.Equal(newSchedules[1].OpensAt, result.Value.ElementAt(1).OpensAt);
        Assert.Equal(newSchedules[1].ClosesAt, result.Value.ElementAt(1).ClosesAt);
    }

    [Fact]
    public async Task Should_Not_CreateMultipleFacilitySchedule()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        TestCreateFacilityScheduleDTO scheduleWithUnknownFacility1 =
            _createFacilityScheduleTest1 with { FacilityId = Guid.NewGuid() };
        TestCreateFacilityScheduleDTO scheduleWithUnknownFacility2 =
            _createFacilityScheduleTest1 with { FacilityId = Guid.NewGuid() };


        List<ICreateFacilityScheduleDTO> newSchedules =
        [
            scheduleWithUnknownFacility1,
            scheduleWithUnknownFacility2
        ];

        // Act
        Result<IEnumerable<FacilitySchedule>> result = await facilityScheduleHandler.CreateMultiple(
            newSchedules
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitySchedulesErrorResults.CreateFacilitySchedules, result.Error);
    }
}
