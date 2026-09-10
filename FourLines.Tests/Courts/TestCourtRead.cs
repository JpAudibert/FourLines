using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Courts;

[Collection(FourLinesCollection.Name)]
public class TestCourtRead(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_GetAllCourts()
    {
        // Arrange
        ICourtHandler courtHandler =
            fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<IEnumerable<Court>> result = await courtHandler.GetAllCourtsFromFacility(TestDataSource.DefaultFacility.Id);

        // Assert
        Assert.NotEmpty(result.Value);
        Assert.Equal(3, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetAllCourts()
    {
        // Arrange
        await using var context = fixtures.CreateContext();
        Facility testFacility = await DbOperations.CreateRecord<Facility>(TestDataSource.Facility3, context);

        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<IEnumerable<Court>> result = await courtHandler.GetAllCourtsFromFacility(testFacility.Id);

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.RetrieveGetCourtDoesNotExist, result.Error);

        await DbOperations.RemoveRecord<Facility>(testFacility.Id, context);
    }

    [Fact]
    public async Task Should_GetFacility()
    {
        // Arrange
        ICourtHandler courtHandler =
            fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<Court> result = await courtHandler.GetFacility(
            TestDataSource.DefaultCourt.FacilityId, TestDataSource.DefaultCourt.Id
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(TestDataSource.DefaultCourt.Id, result.Value.Id);
        Assert.Equal(TestDataSource.DefaultCourt.Name, result.Value.Name);
        Assert.Equal(TestDataSource.DefaultCourt.IsActive, result.Value.IsActive);
        Assert.Equal(TestDataSource.DefaultCourt.FacilityId, result.Value.FacilityId);
        Assert.Equal(TestDataSource.DefaultCourt.SportId, result.Value.SportId);
    }

    [Fact]
    public async Task Should_Not_GetFacility()
    {
        // Arrange
        ICourtHandler courtHandler =
            fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<Court> result = await courtHandler.GetFacility(
            Guid.NewGuid(), TestDataSource.DefaultCourt.Id
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.RetrieveGetCourtDoesNotExist, result.Error);
    }

}
