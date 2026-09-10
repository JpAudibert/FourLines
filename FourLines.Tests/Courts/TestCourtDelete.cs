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
        CourtId = TestDataSource.Court2.Id,
        FacilityId = TestDataSource.Court2.FacilityId,
    };

    [Fact]
    public async Task Should_DeleteCourt()
    {
        // Arrange
        await using var context = fixtures.CreateContext();
        Court testCourt = await DbOperations.CreateRecord<Court>(TestDataSource.ToBeDeletedCourt, context);

        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        TestDeleteCourtDTO deleteCourt = new()
        {
            CourtId = testCourt.Id,
            FacilityId = testCourt.FacilityId,
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
        ICourtHandler courtHandler = fixtures.ServiceProvider.GetRequiredService<ICourtHandler>();

        TestDeleteCourtDTO inexistentCourt = _deleteCourt with { CourtId = Guid.NewGuid() };

        // Act
        Result<bool> result = await courtHandler.Delete(inexistentCourt);

        // Assert
        Assert.False(result.Value);
    }
}
