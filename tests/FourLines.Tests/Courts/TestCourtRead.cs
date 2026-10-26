using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Infrastructure.Contexts;
using FourLines.Tests.Shared;
using FourLines.Tests.Shared.Seed;

namespace FourLines.Tests.Courts;

[Collection(FourLinesCollection.Name)]
public class TestCourtRead(FourLinesFixture fixtures)
{
    [Fact]
    public async Task Should_GetAllCourts_FromFacility()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        FourLinesContext context = scope.ServiceProvider.GetRequiredService<FourLinesContext>();

        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();
        Guid facilityId = FacilitySeed.Default.Id;

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
            FacilitySeed.Dummy.Id
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.RetrieveGetCourtDoesNotExist, result.Error);
    }

    [Fact]
    public async Task Should_GetCourtFromFacility()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        // Act
        Result<Court> result = await courtHandler.GetCourtFromFacility(
            CourtSeed.Default.FacilityId,
            CourtSeed.Default.Id
        );

        // Assert
        Assert.NotNull(result.Value);
        Assert.Equal(CourtSeed.Default.Id, result.Value.Id);
        Assert.Equal(CourtSeed.Default.Name, result.Value.Name);
        Assert.Equal(CourtSeed.Default.IsActive, result.Value.IsActive);
        Assert.Equal(CourtSeed.Default.FacilityId, result.Value.FacilityId);
        Assert.Equal(CourtSeed.Default.SportId, result.Value.SportId);
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
            CourtSeed.Default.Id
        );

        // Assert
        Assert.Null(result.Value);
        Assert.Equal(CourtsErrorResults.RetrieveGetCourtDoesNotExist, result.Error);
    }
}
