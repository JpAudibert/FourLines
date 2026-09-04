using FourLines.Application.DTOs.Courts;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Courts;

public class TestCourtUpdate(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    [Fact]
    public async Task Should_UpdateCourt()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport, context);
            await DbOperations.CreateEntityInMemory<Court>(InMemoryDataSource.Court1, context);
        }

        UpdateCourtDTO updateCourtTest = new()
        {
            Id = InMemoryDataSource.Court1.Id,
            OwnerId = InMemoryDataSource.UserOwner.Id,
            FacilityId = InMemoryDataSource.Facility1.Id,
            SportId = InMemoryDataSource.TestSport.Id,
            Name = "Test Updated Court",
            IsActive = true,
        };

        ICourtHandler courtHandler =
            fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<Court> result = await courtHandler.Update(updateCourtTest);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Court>(result.Value);
        Assert.Equal(updateCourtTest.Name, result.Value.Name);
        Assert.Equal(updateCourtTest.FacilityId, result.Value.FacilityId);
        Assert.Equal(updateCourtTest.SportId, result.Value.SportId);
        Assert.Equal(updateCourtTest.IsActive, result.Value.IsActive);
    }

    [Fact]
    public async Task Should_Not_FindFacility()
    {
        // Arrange
        ICourtHandler courtHandler =
            fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        UpdateCourtDTO updateCourtTest = new()
        {
            Id = InMemoryDataSource.Court1.Id,
            FacilityId = Guid.NewGuid(),
            SportId = InMemoryDataSource.TestSport.Id,
            Name = "Test Updated Court",
            OwnerId = InMemoryDataSource.UserOwner.Id,
        };

        // Act
        Result<Court> result = await courtHandler.Update(updateCourtTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.UpdateUnknownFacility, result.Error);
    }

    [Fact]
    public async Task Should_Not_AffectAnyRowFacility()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.RemoveAllDataFromMemory<Court>(context);
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
        }

        ICourtHandler courtHandler =
            fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        UpdateCourtDTO updateCourtTest = new()
        {
            Id = InMemoryDataSource.Court1.Id,
            FacilityId = InMemoryDataSource.Facility1.Id,
            SportId = InMemoryDataSource.TestSport.Id,
            Name = "Test Updated Court",
            IsActive = true,
            OwnerId = InMemoryDataSource.UserOwner.Id,
        };

        // Act
        Result<Court> result = await courtHandler.Update(updateCourtTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.UpdateCourtDoesNotExist, result.Error);
    }
}
