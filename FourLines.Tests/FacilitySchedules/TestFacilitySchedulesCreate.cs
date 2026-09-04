using FourLines.Application.DTOs.FacilitySchedules;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.FacilitySchedules;

public class TestFacilitySchedulesCreate(InMemoryFixtures fixtures)
    : IClassFixture<InMemoryFixtures>
{

    private readonly static CreateFacilityScheduleDTO _createFacilityScheduleTest1 = new()
    {
        FacilityId = InMemoryDataSource.Facility1.Id,
        OwnerId = InMemoryDataSource.UserOwner.Id,
        DayOfWeek = DayOfWeek.Monday,
        OpensAt = new TimeOnly(9, 0),
        ClosesAt = new TimeOnly(17, 0),
    };

    private readonly static CreateFacilityScheduleDTO _createFacilityScheduleTest2 = new()
    {
        FacilityId = InMemoryDataSource.Facility1.Id,
        OwnerId = InMemoryDataSource.UserOwner.Id,
        DayOfWeek = DayOfWeek.Friday,
        OpensAt = new TimeOnly(9, 0),
        ClosesAt = new TimeOnly(17, 0),
    };

    [Fact]
    public async Task Should_CreateFacilitySchedule()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport, context);
        }

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

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
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.RemoveAllDataFromMemory<Facility>(context);
            await DbOperations.RemoveAllDataFromMemory<FacilitySchedule>(context);
        }

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        // Act
        Result<FacilitySchedule> result = await facilityScheduleHandler.Create(
            _createFacilityScheduleTest1
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitySchedulesErrorResults.CreateFacilitySchedules, result.Error);
    }

    [Fact]
    public async Task Should_CreateMultipleFacilitySchedule()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport, context);
        }

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        List<CreateFacilityScheduleDTO> newSchedules = new List<CreateFacilityScheduleDTO>
        {
            _createFacilityScheduleTest1,
            _createFacilityScheduleTest2,
        };

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
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.RemoveAllDataFromMemory<Facility>(context);
            await DbOperations.RemoveAllDataFromMemory<FacilitySchedule>(context);
        }

        IFacilityScheduleHandler facilityScheduleHandler =
            fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        List<CreateFacilityScheduleDTO> newSchedules =
        [
            _createFacilityScheduleTest1,
            _createFacilityScheduleTest2,
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
