using System.Net.Http.Json;
using Flurl.Http;
using FourLines.Api.Models.RequestModels.Matches;
using FourLines.Tests.Integration.Shared;
using FourLines.Tests.Integration.Shared.Seed;
using Microsoft.AspNetCore.Http;

namespace FourLines.Tests.Integration.ControllersTests;

[Collection(FourLinesCollection.Name)]
public class ShuffleControllerTests(FourLinesFixture fixture)
{
    [Fact]
    public async Task ShuffleTeams_ReturnsOk()
    {
        string token = await ControllerTestHelpers.LoginAsync(
            fixture,
            UserSeed.PlayerToShuffle1.Email
        );
        var reservation = await ControllerTestHelpers.CreateReservationAsync(fixture, token);

        (string Email, bool FixedPosition)[] players =
        [
            (UserSeed.PlayerToShuffle1.Email, true),
            (UserSeed.PlayerToShuffle2.Email, true),
            (UserSeed.PlayerToShuffle3.Email, false),
            (UserSeed.PlayerToShuffle4.Email, false),
            (UserSeed.PlayerToShuffle5.Email, false),
            (UserSeed.PlayerToShuffle6.Email, false),
            (UserSeed.PlayerToShuffle7.Email, false),
            (UserSeed.PlayerToShuffle8.Email, false),
        ];

        foreach ((string email, bool fixedPosition) in players)
        {
            string playerToken = await ControllerTestHelpers.LoginAsync(fixture, email);
            string ingressPath = fixedPosition
                ? "ingress-as-fixed-position"
                : "ingress";
            IFlurlResponse ingress = await fixture.HttpClient.SendAsync(
                url: $"/api/v1/matches/{reservation.Match.Id}/{ingressPath}",
                method: HttpMethod.Post,
                content: JsonContent.Create(
                    new CreateIngressRequest { Code = reservation.Match.Code }
                ),
                token: playerToken
            );

            Assert.Equal(StatusCodes.Status200OK, ingress.StatusCode);
        }

        IFlurlResponse response = await fixture.HttpClient.SendAsync(
            url: $"/api/v1/matches/{reservation.Match.Id}/shuffle",
            method: HttpMethod.Post,
            token: token
        );

        Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
    }
}
