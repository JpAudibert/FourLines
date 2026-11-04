using System.Net.Http.Json;
using Flurl.Http;
using FourLines.Api.Models.RequestModels.Matches;
using FourLines.Tests.Integration.Shared;
using FourLines.Tests.Integration.Shared.Seed;
using Microsoft.AspNetCore.Http;

namespace FourLines.Tests.Integration.ControllersTests;

[Collection(FourLinesCollection.Name)]
public class MatchesControllerTests(FourLinesFixture fixture)
{
    [Fact]
    public async Task GetMatch_ReturnsOk()
    {
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);
        var reservation = await ControllerTestHelpers.CreateReservationAsync(fixture, token);

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/matches/{reservation.Match.Id}",
            method: HttpMethod.Get,
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task Ingress_ReturnsOk()
    {
        string ownerToken = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);
        var reservation = await ControllerTestHelpers.CreateReservationAsync(fixture, ownerToken);
        string playerToken = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player2.Email);

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/matches/{reservation.Match.Id}/ingress",
            method: HttpMethod.Post,
            content: JsonContent.Create(new CreateIngressRequest { Code = reservation.Match.Code }),
            token: playerToken
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task IngressAsFixedPosition_ReturnsOk()
    {
        string ownerToken = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);
        var reservation = await ControllerTestHelpers.CreateReservationAsync(fixture, ownerToken);
        string playerToken = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player2.Email);

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/matches/{reservation.Match.Id}/ingress-as-fixed-position",
            method: HttpMethod.Post,
            content: JsonContent.Create(new CreateIngressRequest { Code = reservation.Match.Code }),
            token: playerToken
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task LeaveMatch_ReturnsOk()
    {
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);
        var reservation = await ControllerTestHelpers.CreateReservationAsync(fixture, token);
        await fixture.HttpClient.SendAsync(
            url: $"/api/v1/matches/{reservation.Match.Id}/ingress",
            method: HttpMethod.Post,
            content: JsonContent.Create(new CreateIngressRequest { Code = reservation.Match.Code }),
            token: token
        );

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/matches/{reservation.Match.Id}/leave",
            method: HttpMethod.Delete,
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateMatchName_ReturnsOk()
    {
        string token = await ControllerTestHelpers.LoginAsync(fixture, UserSeed.Player.Email);
        var reservation = await ControllerTestHelpers.CreateReservationAsync(fixture, token);
        UpdateMatchNameRequest request = new() { Name = "Updated Integration Match" };

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/matches/{reservation.Match.Id}/update-match-name",
            method: HttpMethod.Patch,
            content: JsonContent.Create(request),
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }
}
