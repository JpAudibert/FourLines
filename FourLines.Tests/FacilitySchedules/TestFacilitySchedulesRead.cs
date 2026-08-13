using FourLines.Application.Handlers;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.FacilitySchedules;

public class TestFacilitySchedulesRead(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    [Fact]
    public async Task Should_GetFacilitiesSchedules()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1);
        await _fixtures.CreateEntityInMemory<FacilitySchedule>(
            InMemoryDataSource.FacilitySchedule1
        );
        await _fixtures.CreateEntityInMemory<FacilitySchedule>(
            InMemoryDataSource.FacilitySchedule2
        );

        FacilityScheduleHandler facilityScheduleHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityScheduleHandler>();

        // Act
        Result<IEnumerable<FacilitySchedule>> result = await facilityScheduleHandler.GetSchedules(
            InMemoryDataSource.UserOwner.Id,
            InMemoryDataSource.Facility1.Id
        );

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetFacilitiesSchedules()
    {
        // Arrange
        await _fixtures.RemoveAllDataFromMemory<Facility>();

        FacilityScheduleHandler facilityScheduleHandler =
            _fixtures.ServiceProvider.GetRequiredService<FacilityScheduleHandler>();

        // Act
        Result<IEnumerable<FacilitySchedule>> result = await facilityScheduleHandler.GetSchedules(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitySchedulesErrorResults.RetrieveFacilitySchedules, result.Error);
    }
}
