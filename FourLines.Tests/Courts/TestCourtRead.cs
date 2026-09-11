using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Infrastructure.Contexts;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Courts;

[Collection(FourLinesCollection.Name)]
public class TestCourtRead(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_GetAllCourts()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        FourLinesContext context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();

        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();
        Guid facilityId = TestDataSource.DefaultFacility.Id;

        // Act
        Result<IEnumerable<Court>> result = await courtHandler.GetAllCourtsFromFacility(facilityId);

        // Assert
        int courtsFromFacility = context.Courts.Where(c => c.FacilityId == facilityId).Count();

        Assert.NotEmpty(result.Value);
        Assert.Equal(courtsFromFacility, result.Value.Count());
    }

    [Fact]
    public async Task Should_Not_GetAllCourts()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<IEnumerable<Court>> result = await courtHandler.GetAllCourtsFromFacility(
            TestDataSource.DummyFacility.Id
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.RetrieveGetCourtDoesNotExist, result.Error);

        //await DbOperations.RemoveRecord<Facility>(dummyFacility.Id, fixtures.Context);
    }

    [Fact]
    public async Task Should_GetFacility()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<Court> result = await courtHandler.GetCourtFromFacility(
            TestDataSource.DefaultCourt.FacilityId,
            TestDataSource.DefaultCourt.Id
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
        await using var scope = fixtures.CreateAsyncServiceScope();

        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<Court> result = await courtHandler.GetCourtFromFacility(
            Guid.NewGuid(),
            TestDataSource.DefaultCourt.Id
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.RetrieveGetCourtDoesNotExist, result.Error);
    }
}
