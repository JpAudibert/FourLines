using FourLines.Application.DTOs.Facilities;
using FourLines.Application.DTOs.FacilitySchedules;
using FourLines.Application.Handlers;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;
using Microsoft.Extensions.DependencyInjection;

namespace FourLines.Tests.FacilitySchedules;

public class TestFacilitySchedulesUpdate(InMemoryFixtures fixtures)
    : IClassFixture<InMemoryFixtures>
{
    private readonly InMemoryFixtures _fixtures = fixtures;

    [Fact]
    public async Task Should_UpdateFacilitySchedule()
    {
        // Arrange
        await _fixtures.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner);
        await _fixtures.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner);
        await _fixtures.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1);
        await _fixtures.CreateEntityInMemory<FacilitySchedule>(
            InMemoryDataSource.FacilitySchedule1
        );

        IFacilityScheduleHandler facilityScheduleHandler =
            _fixtures.ServiceProvider.GetRequiredService<IFacilityScheduleHandler>();

        UpdateFacilityScheduleDTO updateFacilityScheduleTest = new()
        {
            Id = InMemoryDataSource.FacilitySchedule1.Id,
            FacilityId = InMemoryDataSource.Facility1.Id,
            OwnerId = InMemoryDataSource.UserOwner.Id,
            DayOfWeek = DayOfWeek.Tuesday,
            OpensAt = new TimeOnly(10, 0),
            ClosesAt = InMemoryDataSource.FacilitySchedule1.ClosesAt,
        };

        // Act
        Result<FacilitySchedule> result = await facilityScheduleHandler.Update(
            updateFacilityScheduleTest
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<FacilitySchedule>(result.Value);
        Assert.Equal(updateFacilityScheduleTest.FacilityId, result.Value.FacilityId);
        Assert.Equal(updateFacilityScheduleTest.DayOfWeek, result.Value.DayOfWeek);
        Assert.Equal(updateFacilityScheduleTest.OpensAt, result.Value.OpensAt);
        Assert.Equal(updateFacilityScheduleTest.ClosesAt, result.Value.ClosesAt);
    }

    [Fact]
    public async Task Should_Not_FindOwnerFacilitySchedule()
    {
        // Arrange
        IFacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        UpdateFacilityDTO updateFacilityTest = new()
        {
            Id = Guid.NewGuid(),
            Name = "Test Updated Facility",
            Address = "123 Test St",
            City = "Test City",
            State = "TS",
            ZipCode = "12345",
            RegistrationNumber = "1234567890",
            OwnerId = Guid.Empty,
        };

        // Act
        Result<Facility> result = await facilityHandler.Update(updateFacilityTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.UpdateEmptyOwnerId, result.Error);
    }

    [Fact]
    public async Task Should_Not_AffectAnyRowFacilitySchedule()
    {
        // Arrange
        IFacilityHandler facilityHandler =
            _fixtures.ServiceProvider.GetRequiredService<IFacilityHandler>();

        UpdateFacilityDTO updateFacilityTest = new()
        {
            Id = Guid.NewGuid(),
            Name = "Test Updated Facility",
            Address = "123 Test St",
            City = "Test City",
            State = "TS",
            ZipCode = "12345",
            RegistrationNumber = "1234567890",
            OwnerId = Guid.NewGuid(),
        };

        // Act
        Result<Facility> result = await facilityHandler.Update(updateFacilityTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(FacilitiesErrorResults.UpdateFacilityDoesNotExist, result.Error);
    }
}
