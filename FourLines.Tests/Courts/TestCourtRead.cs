using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Courts;

public class TestCourtRead(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    [Fact]
    public async Task Should_GetAllCourts()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport, context);
            await DbOperations.CreateEntityInMemory<Court>(InMemoryDataSource.Court1, context);
            await DbOperations.CreateEntityInMemory<Court>(InMemoryDataSource.Court2, context);
        }

        ICourtHandler courtHandler =
            fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<IEnumerable<Court>> result = await courtHandler.GetAllCourtsFromFacility(InMemoryDataSource.Facility1.OwnerId, InMemoryDataSource.Facility1.Id);

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(2, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetAllCourts()
    {
        // Arrange
        ICourtHandler courtHandler =
            fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<IEnumerable<Court>> result = await courtHandler.GetAllCourtsFromFacility(InMemoryDataSource.Facility1.OwnerId, InMemoryDataSource.Facility1.Id);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.RetrieveGetCourtDoesNotExist, result.Error);
    }

    [Fact]
    public async Task Should_GetFacility()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport, context);
            await DbOperations.CreateEntityInMemory<Court>(InMemoryDataSource.Court1, context);
            await DbOperations.CreateEntityInMemory<Court>(InMemoryDataSource.Court2, context);
        }

        ICourtHandler courtHandler =
            fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<Court> result = await courtHandler.GetFacility(
            InMemoryDataSource.Facility1.OwnerId, InMemoryDataSource.Court1.FacilityId, InMemoryDataSource.Court1.Id
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(result.Value.Id, InMemoryDataSource.Court1.Id);
        Assert.Equal(result.Value.Name, InMemoryDataSource.Court1.Name);
        Assert.Equal(result.Value.IsActive, InMemoryDataSource.Court1.IsActive);
        Assert.Equal(result.Value.FacilityId, InMemoryDataSource.Court1.FacilityId);
        Assert.Equal(result.Value.SportId, InMemoryDataSource.Court1.SportId);
    }

    [Fact]
    public async Task Should_Not_GetFacility()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.RemoveAllDataFromMemory<Facility>(context);
        }

        ICourtHandler courtHandler =
            fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<Court> result = await courtHandler.GetFacility(
            InMemoryDataSource.Facility1.OwnerId, InMemoryDataSource.Facility1.Id, InMemoryDataSource.Court1.Id
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.RetrieveGetCourtDoesNotExist, result.Error);
    }

}
