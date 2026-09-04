using FourLines.Application.DTOs.Courts;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Courts;

public class TestCourtCreate(InMemoryFixtures fixtures) : IClassFixture<InMemoryFixtures>
{
    private static CreateCourtDTO _createCourtTest = new()
    {
        OwnerId = InMemoryDataSource.UserOwner.Id,
        FacilityId = InMemoryDataSource.Facility1.Id,
        SportId = InMemoryDataSource.TestSport.Id,
        Name = "Test Court",
        IsActive = true,
    };

    [Fact]
    public async Task Should_CreateCourt()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport, context);

        }

        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<Domain.Models.Court> result = await courtHandler.Create(_createCourtTest);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<Court>(result.Value);
        Assert.Equal(_createCourtTest.Name, result.Value.Name);
        Assert.Equal(_createCourtTest.FacilityId, result.Value.FacilityId);
        Assert.Equal(_createCourtTest.SportId, result.Value.SportId);
        Assert.Equal(_createCourtTest.IsActive, result.Value.IsActive);
    }

    [Fact]
    public async Task Should_Not_HaveFacilityToCreateCourt()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<Sport>(InMemoryDataSource.TestSport, context);
            await DbOperations.RemoveDataFromMemory<Facility>(InMemoryDataSource.Facility1.Id, context);
        }

        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<Court> result = await courtHandler.Create(_createCourtTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.CreateUnknownFacility, result.Error);
    }

    [Fact]
    public async Task Should_Not_HaveKnownSport()
    {
        // Arrange
        await using (var context = fixtures.CreateContext())
        {
            await DbOperations.CreateEntityInMemory<Role>(InMemoryDataSource.RoleOwner, context);
            await DbOperations.CreateEntityInMemory<User>(InMemoryDataSource.UserOwner, context);
            await DbOperations.CreateEntityInMemory<Facility>(InMemoryDataSource.Facility1, context);
            await DbOperations.RemoveAllDataFromMemory<Sport>(context);
        }

        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<Court> result = await courtHandler.Create(_createCourtTest);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.CreateUnknownSport, result.Error);
    }
}
