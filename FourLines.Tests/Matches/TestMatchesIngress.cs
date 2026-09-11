using FourLines.Application.DTOs.Matches.Interfaces;
using FourLines.Application.DTOs.Reservations;
using FourLines.Application.Interfaces;
using FourLines.Domain.Models;
using FourLines.Domain.Results;
using FourLines.Domain.Results.ErrorResults;
using FourLines.Tests.Shared;

namespace FourLines.Tests.Matches;

public record TestCreateIngressDTO : ICreateIngressDTO
{
    public Guid MatchId { get; init; }
    public Guid UserId { get; init; }
    public string Code { get; init; } = default!;
    public bool IngressAsGoalKeeper { get; init; }
}

[Collection(FourLinesCollection.Name)]
public class TestMatchesIngress(FourLinesFixture fixtures)
{
    private TestCreateIngressDTO _ingress = new()
    {
        MatchId = Guid.NewGuid(),
        UserId = Guid.NewGuid(),
        Code = "000000",
        IngressAsGoalKeeper = false,
    };

    [Fact]
    public async Task Should_Ingress_Match_Default()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        await fixtures.EnsureGoalKeeperReservationCreatedAsync();
        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        TestCreateIngressDTO ingress = new()
        {
            MatchId = fixtures.GoalKeeperReservationResult.Value.Match.Id,
            UserId = fixtures.GoalKeeperReservationResult.Value.Reservation.UserId,
            Code = fixtures.GoalKeeperReservationResult.Value.Match.Code,
            IngressAsGoalKeeper = false,
        };

        // Act
        Result<MatchesUsers> result = await matchHandler.Ingress(ingress);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<MatchesUsers>(result.Value);

        Assert.Equal(ingress.MatchId, result.Value.MatchId);
        Assert.Equal(ingress.UserId, result.Value.UserId);
        Assert.Equal(ingress.IngressAsGoalKeeper, result.Value.IsGoalKeeper);
    }

    [Fact]
    public async Task ShouldNot_Ingress_Match_Default_MatchDoesNotExists()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        await fixtures.EnsureGoalKeeperReservationCreatedAsync();

        TestCreateIngressDTO ingress = _ingress with { MatchId = Guid.NewGuid() };

        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<MatchesUsers> result = await matchHandler.Ingress(ingress);

        // Assert
        Assert.NotNull(result.Error);
        Assert.True(result.IsFailure);
        Assert.Equal(MatchesErrorResults.IngressMatchNotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task ShouldNot_Ingress_Match_Default_UserDoesNotExists()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        await fixtures.EnsureGoalKeeperReservationCreatedAsync();
        TestCreateIngressDTO ingress = _ingress with
        {
            MatchId = fixtures.GoalKeeperReservationResult.Value.Match.Id,
            UserId = Guid.NewGuid(),
            Code = fixtures.GoalKeeperReservationResult.Value.Match.Code,
        };

        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<MatchesUsers> result = await matchHandler.Ingress(ingress);

        // Assert
        Assert.NotNull(result.Error);
        Assert.True(result.IsFailure);
        Assert.Equal(MatchesErrorResults.IngressUserNotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task Should_Ingress_Match_As_GoalKeeper()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        await fixtures.EnsureGoalKeeperReservationCreatedAsync();

        TestCreateIngressDTO ingress = _ingress with
        {
            MatchId = fixtures.GoalKeeperReservationResult.Value.Match.Id,
            UserId = TestDataSource.UserPlayer2.Id,
            Code = fixtures.GoalKeeperReservationResult.Value.Match.Code,
            IngressAsGoalKeeper = true,
        };

        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<MatchesUsers> result = await matchHandler.IngressAsGoalKeeper(ingress);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<MatchesUsers>(result.Value);

        Assert.Equal(ingress.MatchId, result.Value.MatchId);
        Assert.Equal(ingress.UserId, result.Value.UserId);
        Assert.Equal(ingress.IngressAsGoalKeeper, result.Value.IsGoalKeeper);
    }

    [Fact]
    public async Task ShouldNot_Ingress_Match_As_GoalKeeper_MatchDoesNotExists()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        await fixtures.EnsureGoalKeeperReservationCreatedAsync();

        TestCreateIngressDTO ingress = _ingress with
        {
            MatchId = Guid.NewGuid(),
            IngressAsGoalKeeper = true,
        };

        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<MatchesUsers> result = await matchHandler.IngressAsGoalKeeper(ingress);

        // Assert
        Assert.NotNull(result.Error);
        Assert.True(result.IsFailure);
        Assert.Equal(MatchesErrorResults.IngressMatchNotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task ShouldNot_Ingress_Match_As_GoalKeeper_UserDoesNotExists()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        await fixtures.EnsureGoalKeeperReservationCreatedAsync();

        TestCreateIngressDTO ingress = _ingress with
        {
            MatchId = fixtures.GoalKeeperReservationResult.Value.Match.Id,
            UserId = Guid.NewGuid(),
            Code = fixtures.GoalKeeperReservationResult.Value.Match.Code,
            IngressAsGoalKeeper = true,
        };

        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<MatchesUsers> result = await matchHandler.IngressAsGoalKeeper(ingress);

        // Assert
        Assert.NotNull(result.Error);
        Assert.True(result.IsFailure);
        Assert.Equal(MatchesErrorResults.IngressUserNotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task ShouldNot_Ingress_Match_As_GoalKeeper_Sport_DoesNot_Have_Fixed_Goal_Keeper()
    {
        // Arrange
        await using var scope = fixtures.CreateAsyncServiceScope();

        await fixtures.EnsureNoGoalKeeperReservationCreatedAsync();

        TestCreateIngressDTO ingress = new()
        {
            MatchId = fixtures.NoGoalKeeperReservationResult.Value.Match.Id,
            UserId = TestDataSource.UserPlayer3.Id,
            Code = fixtures.NoGoalKeeperReservationResult.Value.Match.Code,
            IngressAsGoalKeeper = true,
        };

        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<MatchesUsers> result = await matchHandler.IngressAsGoalKeeper(ingress);

        // Assert
        Assert.NotNull(result.Error);
        Assert.True(result.IsFailure);
        Assert.Equal(
            MatchesErrorResults.IngressSportDoesNotHaveFixedGoalKeeper.Code,
            result.Error.Code
        );
    }
}
