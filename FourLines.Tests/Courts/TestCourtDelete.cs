using FourLines.Application.DTOs.Courts.Interfaces;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Courts;

public record TestDeleteCourtDTO : IDeleteCourtDTO
{
    public Guid CourtId { get; init; }
    public Guid FacilityId { get; init; }
}

[Collection(FourLinesCollection.Name)]
public class TestCourtDelete(FourLinesFixture fixtures)
{
    private static readonly TestDeleteCourtDTO _deleteCourt = new()
    {
        CourtId = TestDataSource.ToBeDeletedCourt.Id,
        FacilityId = TestDataSource.ToBeDeletedCourt.FacilityId,
    };

    [Fact]
    public async Task Should_DeleteCourt()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();
        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        TestDeleteCourtDTO deleteCourt = new()
        {
            CourtId = TestDataSource.ToBeDeletedCourt.Id,
            FacilityId = TestDataSource.ToBeDeletedCourt.FacilityId,
        };

        // Act
        Result<bool> result = await courtHandler.Delete(deleteCourt);

        // Assert
        Assert.True(result.Value);
    }

    [Fact]
    public async Task Should_Not_DeleteCourt()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();
        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        TestDeleteCourtDTO inexistentCourt = _deleteCourt with { CourtId = Guid.NewGuid() };

        // Act
        Result<bool> result = await courtHandler.Delete(inexistentCourt);

        // Assert
        Assert.False(result.Value);
    }
}
