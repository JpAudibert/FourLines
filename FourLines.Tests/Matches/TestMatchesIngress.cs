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
    private bool _isGoalKeeperReservationCreated = false;
    private bool _isNoGoalKeeperReservationCreated = false;
    private static Result<ConfirmReservationResponseDTO> GoalKeeperReservationResult = default!;
    private static Result<ConfirmReservationResponseDTO> NoGoalKeeperReservationResult = default!;
    private async Task EnsureGoalKeeperReservationCreatedAsync()
    {
        if (!_isGoalKeeperReservationCreated)
        {
            IReservationHandler reservationHandler =
                fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();
            GoalKeeperReservationResult = await reservationHandler.Create(TestDataSource.CreateGoalKeeperReservationTest);

            _isGoalKeeperReservationCreated = true;
        }
    }

    private async Task EnsureNoGoalKeeperReservationCreatedAsync()
    {
        if (!_isNoGoalKeeperReservationCreated)
        {
            IReservationHandler reservationHandler =
                fixtures.ServiceProvider.GetRequiredService<IReservationHandler>();
            NoGoalKeeperReservationResult = await reservationHandler.Create(TestDataSource.CreateNoGoalKeeperReservationTest);

            _isNoGoalKeeperReservationCreated = true;
        }
    }

    TestCreateIngressDTO _ingress = new()
    {
        MatchId = GoalKeeperReservationResult.Value.Match.Id,
        UserId = GoalKeeperReservationResult.Value.Reservation.UserId,
        Code = GoalKeeperReservationResult.Value.Match.Code,
        IngressAsGoalKeeper = false,
    };

    [Fact]
    public async Task Should_Ingress_Match_Default()
    {
        // Arrange
        await EnsureGoalKeeperReservationCreatedAsync();
        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<MatchesUsers> result = await matchHandler.Ingress(_ingress);

        // Assert
        Assert.NotNull(result.Value);
        Assert.IsType<MatchesUsers>(result.Value);

        Assert.Equal(_ingress.MatchId, result.Value.MatchId);
        Assert.Equal(_ingress.UserId, result.Value.UserId);
        Assert.Equal(_ingress.IngressAsGoalKeeper, result.Value.IsGoalKeeper);
    }

    [Fact]
    public async Task ShouldNot_Ingress_Match_Default_MatchDoesNotExists()
    {
        // Arrange
        await EnsureGoalKeeperReservationCreatedAsync();
        TestCreateIngressDTO ingress = _ingress with
        {
            MatchId = Guid.NewGuid(),
        };

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
        await EnsureGoalKeeperReservationCreatedAsync();
        TestCreateIngressDTO ingress = _ingress with
        {
            UserId = Guid.NewGuid(),
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
        await EnsureGoalKeeperReservationCreatedAsync();

        TestCreateIngressDTO ingress = _ingress with
        {
            UserId = TestDataSource.UserPlayer2.Id,
            IngressAsGoalKeeper = true,
        };

        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

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
    public async Task ShouldNot_Ingress_Match_As_GoalKeeper_MatchDoesNotExists()
    {
        // Arrange
        await EnsureGoalKeeperReservationCreatedAsync();

        TestCreateIngressDTO ingress = _ingress with
        {
            MatchId = Guid.NewGuid(),
            IngressAsGoalKeeper = true,
        };

        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<MatchesUsers> result = await matchHandler.Ingress(ingress);

        // Assert
        Assert.NotNull(result.Error);
        Assert.True(result.IsFailure);
        Assert.Equal(MatchesErrorResults.IngressMatchNotFound.Code, result.Error.Code);
    }

    [Fact]
    public async Task ShouldNot_Ingress_Match_As_GoalKeeper_UserDoesNotExists()
    {
        // Arrange
        await EnsureGoalKeeperReservationCreatedAsync();

        TestCreateIngressDTO ingress = _ingress with
        {
            UserId = Guid.NewGuid(),
            IngressAsGoalKeeper = true,
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
    public async Task ShouldNot_Ingress_Match_As_GoalKeeper_Sport_DoesNot_Have_Fixed_Goal_Keeper()
    {
        // Arrange
        await EnsureNoGoalKeeperReservationCreatedAsync();

        TestCreateIngressDTO ingress = _ingress with
        {
            MatchId = NoGoalKeeperReservationResult.Value.Match.Id,
            UserId = TestDataSource.UserPlayer3.Id,
            Code = NoGoalKeeperReservationResult.Value.Match.Code,
            IngressAsGoalKeeper = false,
        };

        IMatchHandler matchHandler = fixtures.ServiceProvider.GetRequiredService<IMatchHandler>();

        // Act
        Result<MatchesUsers> result = await matchHandler.IngressAsGoalKeeper(ingress);

        // Assert
        Assert.NotNull(result.Error);
        Assert.True(result.IsFailure);
        Assert.Equal(MatchesErrorResults.IngressSportDoesNotHaveFixedGoalKeeper.Code, result.Error.Code);
    }
}
